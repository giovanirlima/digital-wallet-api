using Digital.Wallet.Events.v1;

public interface ITransactionHandler
{
    Task HandleAsync<T>(T @event, CancellationToken cancellationToken) where T : BaseEvent;
}