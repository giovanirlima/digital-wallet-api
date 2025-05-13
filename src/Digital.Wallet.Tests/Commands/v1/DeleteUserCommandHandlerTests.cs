using Digital.Wallet.Commands.v1.Users.DeleteUser;
using Digital.Wallet.Exceptions;
using Digital.Wallet.Interfaces.v1;
using Digital.Wallet.Tables.v1;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Digital.Wallet.Tests.Commands.v1;

[TestClass]
public class DeleteUserCommandHandlerTests
{
    private readonly Mock<IUserWriteRepository> _userWriteRepositoryMock;
    private readonly DeleteUserCommandHandler _handler;

    public DeleteUserCommandHandlerTests()
    {
        _userWriteRepositoryMock = new Mock<IUserWriteRepository>();
        _handler = new DeleteUserCommandHandler(_userWriteRepositoryMock.Object);
    }

    [TestMethod]
    public async Task Handle_ShouldThrowNotFoundException_WhenUserDoesNotExist()
    {
        var command = new DeleteUserCommand().SetIdProperty(1);

        _userWriteRepositoryMock
            .Setup(repo => repo.GetUserByIdAsync(command.GetIdProperty(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((UserTable?)null);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            _handler.Handle(command, CancellationToken.None));
    }

    [TestMethod]
    public async Task Handle_ShouldDeleteUser_WhenUserExists()
    {
        var command = new DeleteUserCommand().SetIdProperty(1);
        var user = new UserTable { Id = 1 };

        _userWriteRepositoryMock
            .Setup(repo => repo.GetUserByIdAsync(command.GetIdProperty(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _userWriteRepositoryMock
            .Setup(repo => repo.DeleteUserAsync(user, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        await _handler.Handle(command, CancellationToken.None);

        _userWriteRepositoryMock.Verify(repo =>
            repo.DeleteUserAsync(user, It.IsAny<CancellationToken>()), Times.Once);
    }
}