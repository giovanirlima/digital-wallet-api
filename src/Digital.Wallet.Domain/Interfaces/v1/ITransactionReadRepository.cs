using Digital.Wallet.Tables.v1;

namespace Digital.Wallet.Interfaces.v1;

public interface ITransactionReadRepository
{
    Task<IEnumerable<TransactionTable>> GetTransactionByFiltersAsync(DateTime? start, DateTime? end, CancellationToken cancellationToken);
}