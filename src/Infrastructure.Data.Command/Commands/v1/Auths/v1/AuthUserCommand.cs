using Domain.DataTransferObjects.v1;
using MediatR;

namespace Infrastructure.Data.Command.Commands.v1.Auths.v1;

public class AuthUserCommand : IRequest<Auth>
{
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
}