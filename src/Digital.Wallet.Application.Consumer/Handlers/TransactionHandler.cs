using Digital.Wallet.Events.v1;

public class TransactionHandler : ITransactionHandler
{
    private readonly ILogger<TransactionHandler> _logger;
    private readonly ITransactionService _transactionService;

    public TransactionHandler(ILogger<TransactionHandler> logger, ITransactionService transactionService)
    {
        _logger = logger;
        _transactionService = transactionService;
    }

    public async Task HandleAsync<T>(T @event, CancellationToken cancellationToken) where T : BaseEvent
    {
        try
        {
            await _transactionService.ProcessTransactionAsync(@event, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar transação do tipo {EventType}", typeof(T).Name);
            throw;
        }
    }
}