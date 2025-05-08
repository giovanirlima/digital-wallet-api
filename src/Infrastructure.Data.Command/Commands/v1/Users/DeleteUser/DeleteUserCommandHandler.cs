using CrossCutting.Exceptions;
using Infrastructure.Data.Command.Interfaces.v1;
using MediatR;

namespace Infrastructure.Data.Command.Commands.v1.Users.DeleteUser;

public class DeleteUserCommandHandler(IUserCommandRepository userCommandRepository) : IRequestHandler<DeleteUserCommand>
{
    private readonly IUserCommandRepository _userCommandRepository = userCommandRepository;

    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userCommandRepository.GetUserByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("Usuário não encontrado.");

        await _userCommandRepository.DeleteUserAsync(user!, cancellationToken);
    }
}