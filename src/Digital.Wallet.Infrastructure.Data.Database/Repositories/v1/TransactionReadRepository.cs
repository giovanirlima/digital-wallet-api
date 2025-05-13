using Digital.Wallet.Interfaces.v1;
using Digital.Wallet.Selectors;
using Digital.Wallet.Tables.v1;
using Microsoft.EntityFrameworkCore;

namespace Digital.Wallet.Repositories.v1;

public class TransactionReadRepository(ReadOnlyContext readOnlyContext) : ITransactionReadRepository
{
    private readonly ReadOnlyContext _readOnlyContext = readOnlyContext;

    public async Task<IEnumerable<TransactionTable>> GetTransactionByFiltersAsync(DateTime? start, DateTime? end, CancellationToken cancellationToken)
    {
        var query = _readOnlyContext.Transaction
            .Include(t => t.FromUser)
            .Include(t => t.ToUser)
            .AsQueryable();

        if (start.HasValue && end.HasValue)
            query = query.Where(x => x.CreatedAt.Date >= start.Value.Date && x.CreatedAt.Date <= end.Value.Date);

        return await query.ToListAsync(cancellationToken);
    }
}