using Domain.Entities.v1;
using Infrastructure.Data.Query.Interfaces.v1;
using MediatR;

namespace Infrastructure.Data.Query.Queries.v1.Users.GetUserByFilters;

public class GetUserByFiltersQueryHandler(IUserQueryRepository userQueryRepository) : IRequestHandler<GetUserByFiltersQuery, IEnumerable<User>>
{
    private readonly IUserQueryRepository _userQueryRepository = userQueryRepository;

    public async Task<IEnumerable<User>> Handle(GetUserByFiltersQuery request, CancellationToken cancellationToken)
    {
        return await _userQueryRepository.GetUserByFiltersAsync(request, cancellationToken);
    }
}