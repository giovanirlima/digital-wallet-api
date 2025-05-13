using AutoMapper;
using Digital.Wallet.Exceptions;
using Digital.Wallet.Interfaces.v1;
using MediatR;

namespace Digital.Wallet.Commands.v1.Users.UpdateUser;

public class UpdateUserCommandHandler(IMapper mapper, IUserWriteRepository userWriteRepository) : IRequestHandler<UpdateUserCommand>
{
    private readonly IMapper _mapper = mapper;
    private readonly IUserWriteRepository _userWriteRepository = userWriteRepository;

    public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userWriteRepository.GetUserByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("Usuário não encontrado.");

        var updateUser = _mapper.Map(request, user);

        await _userWriteRepository.UpdateUserAsync(updateUser, cancellationToken);
    }
}