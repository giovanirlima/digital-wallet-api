using MediatR;

namespace Infrastructure.Data.Command.Commands.v1.Users.AddUser;

public class AddUserCommandHandler : IRequestHandler<AddUserCommand>
{
    public async Task Handle(AddUserCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}