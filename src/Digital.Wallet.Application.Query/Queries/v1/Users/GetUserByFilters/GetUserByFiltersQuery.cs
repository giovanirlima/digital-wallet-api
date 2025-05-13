using Digital.Wallet.DataTransferObjects.v1;
using MediatR;

namespace Digital.Wallet.Queries.v1.Users.GetUserByFilters;

public class GetUserByFiltersQuery : IRequest<IEnumerable<User>>
{
    public int[]? Id { get; set; }
    public string[]? Name { get; set; }
    public string[]? Email { get; set; }
}