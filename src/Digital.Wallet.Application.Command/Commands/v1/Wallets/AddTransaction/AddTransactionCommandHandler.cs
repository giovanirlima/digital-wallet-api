using Digital.Wallet.Enums.v1;
using Digital.Wallet.Interfaces.v1;
using MediatR;

namespace Digital.Wallet.Commands.v1.Wallets.AddTransaction;

public class AddTransactionCommandHandler(IRabbitMqPublisher rabbitMqPublisher) : IRequestHandler<AddTransactionCommand>
{
    private readonly IRabbitMqPublisher _rabbitMqPublisher = rabbitMqPublisher;

    public async Task Handle(AddTransactionCommand request, CancellationToken cancellationToken)
    {
        await SendMessage(request, request.Transaction, cancellationToken);
    }

    private async Task SendMessage(AddTransactionCommand request, TransactionType transaction, CancellationToken cancellationToken) => await (request.Transaction switch
    {
        TransactionType.Deposit => _rabbitMqPublisher.PublishMessage(
            request, "transactions", "deposit", "deposit_queue", cancellationToken),

        TransactionType.Withdraw => _rabbitMqPublisher.PublishMessage(
            request, "transactions", "withdraw", "withdraw_queue", cancellationToken),

        _ => rabbitMqPublisher.PublishMessage(
            request, "transactions", "transfer", "transfer_queue", cancellationToken)
    });
}