using Domain.Entities.v1;
using MediatR;

namespace Infrastructure.Data.Query.Queries.v1.Users.GetUserByFilters;

public class GetUserByFiltersQuery : IRequest<IEnumerable<User>>
{
    public IEnumerable<int>? Id { get; set; }
    public IEnumerable<string>? Name { get; set; }
    public IEnumerable<string>? Email { get; set; }
}