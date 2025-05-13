using MediatR;

namespace Digital.Wallet.Commands.v1.Users.DeleteUser;

public class DeleteUserCommand : IRequest
{
    internal int Id { get; private set; }

    public DeleteUserCommand SetIdProperty(int id)
    {
        Id = id;
        return this;
    }

    public int GetIdProperty() => Id;
}