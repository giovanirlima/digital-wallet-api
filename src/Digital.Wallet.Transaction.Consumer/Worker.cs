using Digital.Wallet.Transaction.Consumer.Events.v1;
using Domain.Enums.v1;
using Infrastructure.Data.Database.Selectors;
using Infrastructure.Data.Database.Tables.v1;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Digital.Wallet.Transaction.Consumer;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IServiceProvider _serviceProvider;
    private static readonly string[] Queues = { "deposit_queue", "withdraw_queue", "transfer_queue" };

    public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory { HostName = "localhost" };

        using var connection = await factory.CreateConnectionAsync(stoppingToken);
        using var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (model, ea) => await HandleMessageAsync(ea, channel, stoppingToken);

        foreach (var queue in Queues)
            await channel.BasicConsumeAsync(queue, autoAck: false, consumer, stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTime.Now);
            await Task.Delay(1000, stoppingToken);
        }
    }

    private async Task HandleMessageAsync(BasicDeliverEventArgs ea, IChannel channel, CancellationToken cancellationToken)
    {
        try
        {
            var message = Encoding.UTF8.GetString(ea.Body.ToArray());

            await ProcessMessageAsync(ea.RoutingKey, message, cancellationToken);

            await channel.BasicAckAsync(ea.DeliveryTag, multiple: false, cancellationToken);
        }
        catch (Exception ex)
        {
            await channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: true, cancellationToken);

            _logger.LogError(ex, "Erro ao processar mensagem");
        }
    }

    private async Task ProcessMessageAsync(string queueName, string message, CancellationToken cancellationToken)
    {
        switch (queueName)
        {
            case "deposit":
                await HandleEventAsync<DepositEvent>(message, cancellationToken);
                break;
            case "withdraw":
                await HandleEventAsync<WithdrawEvent>(message, cancellationToken);
                break;
            case "transfer":
                await HandleEventAsync<TransferEvent>(message, cancellationToken);
                break;
            default:
                _logger.LogWarning("Fila não reconhecida: {queueName}", queueName);
                break;
        }
    }

    private async Task HandleEventAsync<T>(string message, CancellationToken cancellationToken) where T : BaseEvent
    {
        var teste = typeof(T).Name;
        var eventType = ConvertEvent<T>(message);

        await AddTransactionAsync(eventType, cancellationToken);
    }

    private async Task AddTransactionAsync<T>(T @event, CancellationToken cancellationToken) where T : BaseEvent
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ReadWriteContext>();

        using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

        var fromUser = await context.User.Include(x => x.WalletTable).FirstOrDefaultAsync(x => x.Id == @event.FromUserId, cancellationToken);
        if (fromUser is null) return;

        var toUser = @event.FromUserId != @event.ToUserId
            ? await context.User.Include(x => x.WalletTable).FirstOrDefaultAsync(x => x.Id == @event.ToUserId, cancellationToken)
            : null;

        var transactionTable = CreateTransactionTable(@event, fromUser, toUser, @event.Transaction);

        if (!IsValidOperation(toUser, @event.Transaction, @event.Amount, fromUser.WalletTable!.Amount))
            return;

        ExecuteTransaction(fromUser, toUser, @event.Transaction, @event.Amount);

        context.Wallet.Update(fromUser.WalletTable!);
        if (toUser is not null)
            context.Wallet.Update(toUser.WalletTable!);

        context.Transaction.Add(transactionTable);

        await context.SaveChangesAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);
    }

    private static TransactionTable CreateTransactionTable(BaseEvent @event, UserTable fromUser, UserTable? toUser, TransactionType type) =>
        new()
        {
            FromUserId = fromUser.Id,
            FromWalletId = fromUser.WalletId,
            ToUserId = toUser?.Id ?? fromUser.Id,
            ToWalletId = toUser?.WalletId ?? fromUser.WalletId,
            TransactionType = type,
            Amount = @event.Amount
        };

    private static T ConvertEvent<T>(string message)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        return JsonSerializer.Deserialize<T>(message, options);
    }

    private static bool IsValidOperation(UserTable? toUser, TransactionType transaction, decimal requestAmount, decimal availableAmount) => transaction switch
    {
        TransactionType.Deposit => true,
        TransactionType.Withdraw => requestAmount <= availableAmount,
        TransactionType.Transfer => requestAmount <= availableAmount && toUser is not null,
        _ => false
    };

    private void ExecuteTransaction(UserTable from, UserTable? to, TransactionType type, decimal amount)
    {
        from.WalletTable!.Amount += type switch
        {
            TransactionType.Deposit => amount,
            TransactionType.Withdraw => -amount,
            TransactionType.Transfer => -amount,
            _ => 0
        };

        if (type == TransactionType.Transfer && to is not null)
            to.WalletTable!.Amount += amount;
    }
}