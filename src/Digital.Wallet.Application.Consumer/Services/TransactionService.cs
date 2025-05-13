using Digital.Wallet.Events.v1;
using Digital.Wallet.Selectors;

public class TransactionService(ReadWriteContext readWriteContext, ILogger<TransactionService> logger) : ITransactionService
{
    private readonly ILogger _logger = logger;
    private readonly ReadWriteContext _readWriteContext = readWriteContext;

    public async Task ProcessTransactionAsync<T>(T @event, CancellationToken cancellationToken) where T : BaseEvent
    {
        using var transaction = await _readWriteContext.Database.BeginTransactionAsync(cancellationToken);

        var operation = await TransactionOperation.Create(@event, _readWriteContext);

        if (!operation.IsValid())
        {
            _logger.LogWarning("Operação inválida para o evento {EventType}", typeof(T).Name);
            return;
        }

        operation.Execute();

        _readWriteContext.Wallet.Update(operation.FromWallet);

        if (operation.ToWallet != null)
            _readWriteContext.Wallet.Update(operation.ToWallet);

        _readWriteContext.Transaction.Add(operation.ToTransactionTable());

        await _readWriteContext.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
    }
}