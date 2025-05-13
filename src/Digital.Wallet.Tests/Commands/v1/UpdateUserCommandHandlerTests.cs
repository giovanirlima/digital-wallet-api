using AutoMapper;
using Digital.Wallet.Commands.v1.Users.UpdateUser;
using Digital.Wallet.Exceptions;
using Digital.Wallet.Interfaces.v1;
using Digital.Wallet.Tables.v1;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Digital.Wallet.Tests.Commands.v1;

[TestClass]
public class UpdateUserCommandHandlerTests
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IUserWriteRepository> _userWriteRepositoryMock;
    private readonly UpdateUserCommandHandler _handler;

    public UpdateUserCommandHandlerTests()
    {
        _mapperMock = new Mock<IMapper>();
        _userWriteRepositoryMock = new Mock<IUserWriteRepository>();
        _handler = new UpdateUserCommandHandler(_mapperMock.Object, _userWriteRepositoryMock.Object);
    }

    [TestMethod]
    public async Task Handle_ShouldThrowNotFoundException_WhenUserDoesNotExist()
    {
        var command = new UpdateUserCommand().SetIdProperty(1);

        _userWriteRepositoryMock
            .Setup(repo => repo.GetUserByIdAsync(command.GetIdProperty(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((UserTable?)null);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            _handler.Handle(command, CancellationToken.None));
    }

    [TestMethod]
    public async Task Handle_ShouldUpdateUser_WhenUserExists()
    {
        var command = new UpdateUserCommand { Name = "Updated Name" }.SetIdProperty(1);
        var existingUser = new UserTable { Id = command.GetIdProperty(), Name = "Old Name" };
        var updatedUser = new UserTable { Id = command.GetIdProperty(), Name = command.Name };

        _userWriteRepositoryMock
            .Setup(repo => repo.GetUserByIdAsync(command.GetIdProperty(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingUser);

        _mapperMock
            .Setup(mapper => mapper.Map(command, existingUser))
            .Returns(updatedUser);

        _userWriteRepositoryMock
            .Setup(repo => repo.UpdateUserAsync(updatedUser, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        await _handler.Handle(command, CancellationToken.None);

        _userWriteRepositoryMock.Verify(repo =>
            repo.UpdateUserAsync(updatedUser, It.IsAny<CancellationToken>()), Times.Once);
    }
}