using Digital.Wallet.Settings;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

public class Worker(ILogger<Worker> logger, IServiceProvider serviceProvider) : BackgroundService
{
    private readonly ILogger _logger = logger;
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly Dictionary<string, Type> _consumerTypes =
        new()
        {
            { "deposit_queue", typeof(DepositConsumer) },
            { "withdraw_queue", typeof(WithdrawConsumer) },
            { "transfer_queue", typeof(TransferConsumer) }
        };

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory { HostName = AppSettings.RabbitMqSettings.HostName };

        using var connection = await factory.CreateConnectionAsync(stoppingToken);
        using var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

        foreach (var (queueName, consumerType) in _consumerTypes)
        {
            var consumer = (BaseConsumer)ActivatorUtilities.CreateInstance(
                _serviceProvider, consumerType);

            var asyncConsumer = new AsyncEventingBasicConsumer(channel);

            await CreateQueueAsync(channel, queueName, queueName.Replace("_queue", ""), stoppingToken);

            asyncConsumer.ReceivedAsync += async (model, ea) =>
                await consumer.HandleMessageAsync(ea, channel, stoppingToken);

            await channel.BasicConsumeAsync(queueName, autoAck: false, asyncConsumer, stoppingToken);
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTime.Now);
            await Task.Delay(1000, stoppingToken);
        }
    }

    private async Task CreateQueueAsync(IChannel channel, string queueName, string routingKey, CancellationToken cancellationToken)
    {
        await channel.ExchangeDeclareAsync(
            exchange: "transactions",
            type: ExchangeType.Direct,
            durable: true,
            autoDelete: false,
            arguments: null,
            cancellationToken: cancellationToken
        );

        await channel.QueueDeclareAsync(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: cancellationToken
        );

        await channel.QueueBindAsync(
            queue: queueName,
            exchange: "transactions",
            routingKey: routingKey,
            arguments: null,
            cancellationToken: cancellationToken
        );
    }
}