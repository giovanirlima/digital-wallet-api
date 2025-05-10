using Domain.DataTransferObjects.v1;
using MediatR;

namespace Infrastructure.Data.Command.Commands.v1.Users.AddUser;

public class AddUserCommand : IRequest
{
    public string Name { get; set; } = default!;
    public string Password { get; set; } = default!;
    public DateTime Birthday { get; set; }
    public string Email { get; set; } = default!;
    public Address Address { get; set; } = default!;
}