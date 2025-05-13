using AutoMapper;
using Digital.Wallet.DataTransferObjects.v1;
using Digital.Wallet.Interfaces.v1;
using MediatR;

namespace Digital.Wallet.Queries.v1.Users.GetUserByFilters;

public class GetUserByFiltersQueryHandler(IMapper mapper, IUserReadRepository userReadRepository) : IRequestHandler<GetUserByFiltersQuery, IEnumerable<User>>
{
    private readonly IMapper _mapper = mapper;
    private readonly IUserReadRepository _userReadRepository = userReadRepository;

    public async Task<IEnumerable<User>> Handle(GetUserByFiltersQuery request, CancellationToken cancellationToken)
    {
        var response = await _userReadRepository.GetUserByFiltersAsync(request?.Id, request?.Name, request?.Email, cancellationToken);

        return _mapper.Map<IEnumerable<User>>(response);
    }
}