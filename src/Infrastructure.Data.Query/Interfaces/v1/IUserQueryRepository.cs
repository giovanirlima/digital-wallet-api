using Domain.Entities.v1;
using Infrastructure.Data.Query.Queries.v1.Users.GetUserByFilters;

namespace Infrastructure.Data.Query.Interfaces.v1;

public interface IUserQueryRepository
{
    Task<IEnumerable<User>> GetUserByFiltersAsync(GetUserByFiltersQuery request, CancellationToken cancellationToken);
}