using AutoMapper;
using Digital.Wallet.Exceptions;
using Digital.Wallet.Interfaces.v1;
using Digital.Wallet.Tables.v1;
using MediatR;

namespace Digital.Wallet.Commands.v1.Users.AddUser;

public class AddUserCommandHandler(IMapper mapper, IUserWriteRepository userWriteRepository) : IRequestHandler<AddUserCommand>
{
    private readonly IMapper _mapper = mapper;
    private readonly IUserWriteRepository _userWriteRepository = userWriteRepository;

    public async Task Handle(AddUserCommand request, CancellationToken cancellationToken)
    {
        var userExists = await _userWriteRepository.GetUserByEmailAsync(request.Email, cancellationToken);

        if (userExists is not null)
            throw new BadRequestException("Email informado já está cadastrado.");

        var user = _mapper.Map<UserTable>(request);

        await _userWriteRepository.AddUserAsync(user, cancellationToken);
    }
}