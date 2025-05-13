using Digital.Wallet.Exceptions;
using Digital.Wallet.Interfaces.v1;
using MediatR;

namespace Digital.Wallet.Commands.v1.Users.DeleteUser;

public class DeleteUserCommandHandler(IUserWriteRepository userWriteRepository) : IRequestHandler<DeleteUserCommand>
{
    private readonly IUserWriteRepository _userWriteRepository = userWriteRepository;

    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userWriteRepository.GetUserByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("Usuário não encontrado.");

        await _userWriteRepository.DeleteUserAsync(user!, cancellationToken);
    }
}