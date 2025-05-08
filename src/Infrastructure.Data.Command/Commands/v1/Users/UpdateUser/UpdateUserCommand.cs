using MediatR;

namespace Infrastructure.Data.Command.Commands.v1.Users.UpdateUser;

public class UpdateUserCommand : IRequest
{
    internal int Id { get; private set; }
    public string? Name { get; set; } = default!;
    public DateTime? Birthday { get; set; }
    public string? Email { get; set; } = default!;

    public UpdateUserCommand SetIdProperty(int id)
    {
        Id = id;
        return this;
    }
}