using RabbitMQ.Client;
using RabbitMQ.Client.Events;

public abstract class BaseConsumer
{
    protected readonly ILogger<BaseConsumer> _logger;
    protected readonly IServiceProvider _serviceProvider;

    protected BaseConsumer(ILogger<BaseConsumer> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public abstract Task HandleMessageAsync(BasicDeliverEventArgs ea, IChannel channel, CancellationToken cancellationToken);
}