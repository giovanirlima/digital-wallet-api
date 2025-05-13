using Digital.Wallet.DataTransferObjects.v1;
using Digital.Wallet.Exceptions;
using Digital.Wallet.Interfaces.v1;
using Digital.Wallet.Services.v1;
using MediatR;

namespace Digital.Wallet.Commands.v1.Auths;

public class AuthUserCommandHandler(IUserWriteRepository userWriteRepository, IAuthService authService) : IRequestHandler<AuthUserCommand, AuthToken>
{
    private readonly IUserWriteRepository _userWriteRepository = userWriteRepository;
    private readonly IAuthService _authService = authService;

    public async Task<AuthToken> Handle(AuthUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userWriteRepository.GetUserByEmailAsync(request.Email, cancellationToken)
            ?? throw new NotFoundException("Usuario não encontrado.");

        if (!request.Password.Verify(user.Password))
            throw new UnauthorizedException("Senha incorreta.");

        return new()
        {
            Token = _authService.GenerateToken(user.Email),
        };
    }
}