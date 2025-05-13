using Digital.Wallet.Extensions;
using Digital.Wallet.Interfaces.v1;
using Digital.Wallet.Selectors;
using Digital.Wallet.Tables.v1;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Query.Repositories.v1;

public class UserReadRepository(ReadOnlyContext readOnlyContext) : IUserReadRepository
{
    private readonly ReadOnlyContext _readOnlyContext = readOnlyContext;

    public async Task<IEnumerable<UserTable>> GetUserByFiltersAsync(IEnumerable<int>? id, IEnumerable<string>? name, IEnumerable<string>? email, CancellationToken cancellationToken)
    {
        var query = _readOnlyContext.User
            .Include(u => u.WalletTable)
            .Include(u => u.AddressTable)
            .Include(u => u.SentTransaction)
                .ThenInclude(t => t.ToUser)
            .Include(u => u.ReceivedTransaction)
                .ThenInclude(t => t.FromUser)
            .AsQueryable();

        if (id?.Any() is true)
            query = query.Where(x => id.Contains(x.Id));

        if (name?.Any() is true)
            query = query.WhereLikeAny(x => x.Name.ToLower(), name);

        if (email?.Any() is true)
            query = query.WhereLikeAny(x => x.Email, email);

        return await query.ToListAsync(cancellationToken);
    }
}