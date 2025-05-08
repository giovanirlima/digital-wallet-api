using Domain.Entities.v1;
using Domain.Extensions;
using Infrastructure.Data.Database.Selectors;
using Infrastructure.Data.Query.Interfaces.v1;
using Infrastructure.Data.Query.Queries.v1.Users.GetUserByFilters;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Query.Repositories.v1;

public class UserQueryRepository(ReadOnlyContext context) : IUserQueryRepository
{
    private readonly ReadOnlyContext _context = context;

    public async Task<IEnumerable<User>> GetUserByFiltersAsync(GetUserByFiltersQuery request, CancellationToken cancellationToken)
    {
        var query = _context.User.AsQueryable();

        if (request?.Id?.Any() is true)
            query = query.Where(x => request.Id.Contains(x.Id));

        if (request?.Name?.Any() is true)
            query = query.WhereLikeAny(x => x.Name.ToLower(), request.Name);

        if (request?.Email?.Any() is true)
            query = query.WhereLikeAny(x => x.Email, request.Email);

        return await query.Include(x => x.Address).ToListAsync(cancellationToken);
    }
}