using System.Text;
using System.Text.Json;
using Digital.Wallet.Events.v1;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

public class TransferConsumer(ILogger<TransferConsumer> logger, IServiceProvider serviceProvider) : BaseConsumer(logger, serviceProvider)
{
    public override async Task HandleMessageAsync(BasicDeliverEventArgs ea, IChannel channel, CancellationToken cancellationToken)
    {
        try
        {
            var message = Encoding.UTF8.GetString(ea.Body.ToArray());
            var @event = JsonSerializer.Deserialize<TransferEvent>(message);

            using var scope = _serviceProvider.CreateScope();
             var handler = scope.ServiceProvider.GetRequiredService<ITransactionHandler>();
            await handler.HandleAsync(@event, cancellationToken);

            await channel.BasicAckAsync(ea.DeliveryTag, multiple: false, cancellationToken);
        }
        catch (Exception ex)
        {
            await channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: true, cancellationToken);
            _logger.LogError(ex, "Erro ao processar mensagem de transferência.");
        }
    }
}