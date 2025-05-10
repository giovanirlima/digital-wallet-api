using Domain.Extensions;
using Infrastructure.Data.Database.Selectors;
using Infrastructure.Data.Database.Tables.v1;
using Infrastructure.Data.Query.Interfaces.v1;
using Infrastructure.Data.Query.Queries.v1.Users.GetUserByFilters;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Query.Repositories.v1;

public class UserQueryRepository(ReadOnlyContext context) : IUserQueryRepository
{
    private readonly ReadOnlyContext _context = context;

    public async Task<IEnumerable<UserTable>> GetUserByFiltersAsync(GetUserByFiltersQuery request, CancellationToken cancellationToken)
    {
        var query = _context.User
            .Include(u => u.WalletTable)
            .Include(u => u.AddressTable)
            .Include(u => u.SentTransaction)
                .ThenInclude(t => t.ToUser)
            .Include(u => u.ReceivedTransaction)
                .ThenInclude(t => t.FromUser)
            .AsQueryable();

        if (request?.Id?.Any() is true)
            query = query.Where(x => request.Id.Contains(x.Id));

        if (request?.Name?.Any() is true)
            query = query.WhereLikeAny(x => x.Name.ToLower(), request.Name);

        if (request?.Email?.Any() is true)
            query = query.WhereLikeAny(x => x.Email, request.Email);

        if (request!.StartTransactionDate.HasValue)
            query = query.Where(x => x.SentTransaction.Any(t => t.CreatedAt >= request.StartTransactionDate.Value.Date || x.ReceivedTransaction.Any(t => t.CreatedAt >= request.StartTransactionDate.Value.Date)));

        if (request!.EndTransactionDate.HasValue)
            query = query.Where(x => x.SentTransaction.Any(t => t.CreatedAt <= request.EndTransactionDate.Value.Date));

        return await query.ToListAsync(cancellationToken);
    }
}