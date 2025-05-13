using Digital.Wallet.Commands.v1.Auths;
using Digital.Wallet.Exceptions;
using Digital.Wallet.Interfaces.v1;
using Digital.Wallet.Services.v1;
using Digital.Wallet.Tables.v1;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Digital.Wallet.Tests.Commands.v1;

[TestClass]
public class AuthUserCommandHandlerTests
{
    private Mock<IUserWriteRepository> _userWriteRepositoryMock;
    private Mock<IAuthService> _authServiceMock;
    private AuthUserCommandHandler _handler;
    private CancellationToken _cancellationToken = default!;

    [TestInitialize]
    public void Initialize()
    {
        _userWriteRepositoryMock = new Mock<IUserWriteRepository>();
        _authServiceMock = new Mock<IAuthService>();

        _handler = new AuthUserCommandHandler(
            _userWriteRepositoryMock.Object,
            _authServiceMock.Object
        );
    }

    [TestMethod]
    public async Task ValidRequestReturnsToken()
    {
        var command = new AuthUserCommand { Email = "mock@mock.com", Password = "mock@mock.com" };
        var user = new UserTable { Email = "mock@mock.com", Password = "mock@mock.com".Encrypt() };

        _userWriteRepositoryMock
            .Setup(x => x.GetUserByEmailAsync(command.Email, _cancellationToken))
            .ReturnsAsync(user);

        _authServiceMock.Setup(x => x.GenerateToken(command.Email)).Returns("GeneratedToken");

        var result = await _handler.Handle(command, CancellationToken.None);

        IsNotNull(result);
        AreEqual("GeneratedToken", result.Token);
        _userWriteRepositoryMock.Verify(x => x.GetUserByEmailAsync(command.Email, _cancellationToken), Times.Once);
        _authServiceMock.Verify(x => x.GenerateToken(command.Email), Times.Once);
    }

    [TestMethod]
    public async Task UserNotFoundThrowsNotFoundException()
    {
        var command = new AuthUserCommand { Email = "notfound@example.com", Password = "notfound@example.com" };

        _userWriteRepositoryMock
            .Setup(x => x.GetUserByEmailAsync(command.Email, _cancellationToken))
            .ReturnsAsync((UserTable)null);

        await ThrowsExactlyAsync<NotFoundException>(async () => { await _handler.Handle(command, CancellationToken.None); });

        _userWriteRepositoryMock.Verify(x => x.GetUserByEmailAsync(command.Email, _cancellationToken), Times.Once);
        _authServiceMock.Verify(x => x.GenerateToken(It.IsAny<string>()), Times.Never);
    }

    [TestMethod]
    public async Task InvalidPasswordThrowsBadRequestException()
    {
        var command = new AuthUserCommand { Email = "test@example.com", Password = "testfail@example.com" };
        var user = new UserTable { Email = "test@example.com", Password = "test@example.com".Encrypt() };

        _userWriteRepositoryMock
            .Setup(x => x.GetUserByEmailAsync(command.Email, _cancellationToken))
            .ReturnsAsync(user);

        await ThrowsExactlyAsync<UnauthorizedException>(async () => { await _handler.Handle(command, CancellationToken.None); });

        _userWriteRepositoryMock.Verify(x => x.GetUserByEmailAsync(command.Email, _cancellationToken), Times.Once);
        _authServiceMock.Verify(x => x.GenerateToken(It.IsAny<string>()), Times.Never);
    }
}