using Domain.Entities.v1;
using Domain.Extensions;
using Infrastructure.Data.Database.Selectors;
using Infrastructure.Data.Query.Interfaces.v1;
using Infrastructure.Data.Query.Queries.v1.Users.GetUserByFilters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Data.Query.Repositories.v1;

public class UserQueryRepository(IServiceProvider serviceProvider) : IUserQueryRepository
{
    private readonly ReadOnlyContext _context = serviceProvider.CreateScope()
        .ServiceProvider.GetRequiredService<ReadOnlyContext>();

    public async Task<IEnumerable<User>> GetUserByFiltersAsync(GetUserByFiltersQuery request, CancellationToken cancellationToken)
    {
        var query = _context.User.AsQueryable();

        if (request?.Id?.Count() > 0)
            query = query.Where(x => request.Id.Equals(x.Id));

        if (request?.Name?.Count() > 0)
            query = query.WhereLikeAny(x => x.Name, request.Name);

        if (request?.Email?.Count() > 0)
            query = query.WhereLikeAny(x => x.Email, request.Email);

        return await query.Include(x => x.Address).ToListAsync(cancellationToken);
    }
}