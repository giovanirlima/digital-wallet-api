using System.Text;
using System.Text.Json;
using Digital.Wallet.Interfaces.v1;
using Digital.Wallet.Settings;
using RabbitMQ.Client;

namespace Digital.Wallet.Publishers.v1;

public class RabbitMqPublisher(IConnection connection) : IRabbitMqPublisher
{
    public async Task PublishMessage<T>(T message, string exchangeName, string routingKey, string queueName, CancellationToken cancellationToken) where T : class
    {
        using var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

        await channel.ExchangeDeclareAsync(
            exchange: exchangeName,
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
            exchange: exchangeName,
            routingKey: routingKey,
            arguments: null,
            cancellationToken: cancellationToken
        );

        await channel.BasicPublishAsync(
            exchange: exchangeName,
            routingKey: routingKey,
            mandatory: false,
            basicProperties: new BasicProperties { Persistent = true },
            body: Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message)),
            cancellationToken: cancellationToken
        );
    }
}