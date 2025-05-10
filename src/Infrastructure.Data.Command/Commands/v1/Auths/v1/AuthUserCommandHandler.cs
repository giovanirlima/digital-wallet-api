using CrossCutting.Exceptions;
using Domain.DataTransferObjects.v1;
using Domain.Extensions;
using Domain.Interfaces.v1;
using Infrastructure.Data.Command.Interfaces.v1;
using MediatR;

namespace Infrastructure.Data.Command.Commands.v1.Auths.v1;

public class AuthUserCommandHandler(IUserCommandRepository userCommandRepository, IAuthService authService) : IRequestHandler<AuthUserCommand, Auth>
{
    private readonly IUserCommandRepository _userCommandRepository = userCommandRepository;
    private readonly IAuthService _authService = authService;

    public async Task<Auth> Handle(AuthUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userCommandRepository.GetUserByEmailAsync(request.Email, cancellationToken)
            ?? throw new NotFoundException("Usuario não encontrado.");

        if (!request.Password.Verify(user.Password))
            throw new UnauthorizedException("Senha incorreta.");

        return new()
        {
            Token = _authService.GenerateToken(user.Email),
        };
    }
}