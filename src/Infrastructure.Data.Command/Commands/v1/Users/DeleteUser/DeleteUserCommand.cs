using MediatR;

namespace Infrastructure.Data.Command.Commands.v1.Users.DeleteUser;

public class DeleteUserCommand : IRequest
{
    internal int Id { get; private set; }

    public DeleteUserCommand SetIdProperty(int id)
    {
        Id = id;
        return this;
    }
}