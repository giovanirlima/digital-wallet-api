namespace Domain.Interfaces.v1;

public interface IRabbitMqPublisher
{
    Task PublishMessage<T>(T message, string exchangeName, string routingKey, string queueName, CancellationToken cancellationToken) where T : class;
}