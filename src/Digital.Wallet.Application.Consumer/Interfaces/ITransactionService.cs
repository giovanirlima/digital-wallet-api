using Digital.Wallet.Events.v1;

public interface ITransactionService
{
    Task ProcessTransactionAsync<T>(T @event, CancellationToken cancellationToken) where T : BaseEvent;
}