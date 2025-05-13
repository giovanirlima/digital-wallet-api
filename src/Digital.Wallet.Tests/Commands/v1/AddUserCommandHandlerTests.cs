using AutoMapper;
using Digital.Wallet.Commands.v1.Users.AddUser;
using Digital.Wallet.Exceptions;
using Digital.Wallet.Interfaces.v1;
using Digital.Wallet.Tables.v1;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Digital.Wallet.Tests.Commands.v1;

[TestClass]
public class AddUserCommandHandlerTests
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IUserWriteRepository> _userWriteRepositoryMock;
    private readonly AddUserCommandHandler _handler;

    public AddUserCommandHandlerTests()
    {
        _mapperMock = new Mock<IMapper>();
        _userWriteRepositoryMock = new Mock<IUserWriteRepository>();
        _handler = new AddUserCommandHandler(_mapperMock.Object, _userWriteRepositoryMock.Object);
    }

    [TestMethod]
    public async Task Handle_ShouldThrowBadRequestException_WhenEmailAlreadyExists()
    {
        var request = new AddUserCommand { Email = "existing@example.com" };
        var existingUser = new UserTable { Email = request.Email };

        _userWriteRepositoryMock
            .Setup(repo => repo.GetUserByEmailAsync(request.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingUser);

        await Assert.ThrowsAsync<BadRequestException>(() =>
            _handler.Handle(request, CancellationToken.None));
    }

    [TestMethod]
    public async Task Handle_ShouldAddUser_WhenEmailDoesNotExist()
    {
        // Arrange
        var request = new AddUserCommand { Email = "new@example.com" };
        var userTable = new UserTable { Email = request.Email };

        _userWriteRepositoryMock
            .Setup(repo => repo.GetUserByEmailAsync(request.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync((UserTable?)null);

        _mapperMock
            .Setup(mapper => mapper.Map<UserTable>(request))
            .Returns(userTable);

        _userWriteRepositoryMock
            .Setup(repo => repo.AddUserAsync(userTable, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        await _handler.Handle(request, CancellationToken.None);

        _userWriteRepositoryMock.Verify(repo =>
            repo.AddUserAsync(userTable, It.IsAny<CancellationToken>()), Times.Once);
    }
}