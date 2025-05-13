using Digital.Wallet.Commands.v1.Wallets.AddTransaction;
using Digital.Wallet.Enums.v1;
using Digital.Wallet.Interfaces.v1;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Xunit;

namespace Digital.Wallet.Tests.Commands.v1;

[TestClass]
public class AddTransactionCommandHandlerTests
{
    private readonly Mock<IRabbitMqPublisher> _rabbitMqPublisherMock;
    private readonly AddTransactionCommandHandler _handler;

    public AddTransactionCommandHandlerTests()
    {
        _rabbitMqPublisherMock = new Mock<IRabbitMqPublisher>();
        _handler = new AddTransactionCommandHandler(_rabbitMqPublisherMock.Object);
    }

    [TestMethod]
    public async Task Handle_WithDepositTransaction_ShouldPublishToDepositQueue()
    {
        var command = new AddTransactionCommand
        {
            Transaction = TransactionType.Deposit
        };

        var cancellationToken = CancellationToken.None;

        await _handler.Handle(command, cancellationToken);

        _rabbitMqPublisherMock.Verify(
            x => x.PublishMessage(
                command,
                "transactions",
                "deposit",
                "deposit_queue",
                cancellationToken),
            Times.Once);
    }

    [TestMethod]
    public async Task Handle_WithWithdrawTransaction_ShouldPublishToWithdrawQueue()
    {
        var command = new AddTransactionCommand
        {
            Transaction = TransactionType.Withdraw
        };

        var cancellationToken = CancellationToken.None;

        await _handler.Handle(command, cancellationToken);

        _rabbitMqPublisherMock.Verify(
            x => x.PublishMessage(
                command,
                "transactions",
                "withdraw",
                "withdraw_queue",
                cancellationToken),
            Times.Once);
    }

    [TestMethod]
    public async Task Handle_WithTransferTransaction_ShouldPublishToTransferQueue()
    {
        var command = new AddTransactionCommand
        {
            Transaction = TransactionType.Transfer
        };

        var cancellationToken = CancellationToken.None;

        await _handler.Handle(command, cancellationToken);

        _rabbitMqPublisherMock.Verify(
            x => x.PublishMessage(
                command,
                "transactions",
                "transfer",
                "transfer_queue",
                cancellationToken),
            Times.Once);
    }

    [TestMethod]
    public async Task Handle_WithDefaultCase_ShouldPublishToTransferQueue()
    {
        var command = new AddTransactionCommand
        {
            Transaction = (TransactionType)999
        };

        var cancellationToken = CancellationToken.None;

        await _handler.Handle(command, cancellationToken);

        _rabbitMqPublisherMock.Verify(
            x => x.PublishMessage(
                command,
                "transactions",
                "transfer",
                "transfer_queue",
                cancellationToken),
            Times.Once);
    }
}