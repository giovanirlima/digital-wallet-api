using AutoMapper;
using Domain.DataTransferObjects.v1;
using Infrastructure.Data.Query.Interfaces.v1;

namespace Infrastructure.Data.Query.Queries.v1.Users.GetUserByFilters;

public class GetUserByFiltersQueryHandler(IMapper mapper, IUserQueryRepository userQueryRepository) : IRequestHandler<GetUserByFiltersQuery, IEnumerable<User>>
{
    private readonly IMapper _mapper = mapper;
    private readonly IUserQueryRepository _userQueryRepository = userQueryRepository;

    public async Task<IEnumerable<User>> Handle(GetUserByFiltersQuery request, CancellationToken cancellationToken)
    {
        var response = await _userQueryRepository.GetUserByFiltersAsync(request, cancellationToken);

        return _mapper.Map<IEnumerable<User>>(response);
    }
}