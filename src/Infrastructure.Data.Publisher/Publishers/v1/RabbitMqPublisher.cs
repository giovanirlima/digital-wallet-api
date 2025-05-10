using CrossCutting.Settings;
using Domain.Interfaces.v1;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Infrastructure.Data.Publisher.Publishers.v1;

public class RabbitMqPublisher : IRabbitMqPublisher
{
    public async Task PublishMessage<T>(T message, string exchangeName, string routingKey, string queueName, CancellationToken cancellationToken) where T : class
    {
        var factory = new ConnectionFactory
        {
            HostName = AppSettings.RabbitMqSettings.HostName,
            UserName = AppSettings.RabbitMqSettings.Username,
            Password = AppSettings.RabbitMqSettings.Password,
        };

        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

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