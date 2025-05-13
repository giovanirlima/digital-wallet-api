using Digital.Wallet.DataTransferObjects.v1;
using MediatR;

namespace Digital.Wallet.Commands.v1.Auths;

public class AuthUserCommand : IRequest<AuthToken>
{
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
}