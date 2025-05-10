global using MediatR;
using Domain.DataTransferObjects.v1;

namespace Infrastructure.Data.Query.Queries.v1.Users.GetUserByFilters;

public class GetUserByFiltersQuery : IRequest<IEnumerable<User>>
{
    public int[]? Id { get; set; }
    public string[]? Name { get; set; }
    public string[]? Email { get; set; }
    public DateTime? StartTransactionDate { get; set; }
    public DateTime? EndTransactionDate { get; set; }
}