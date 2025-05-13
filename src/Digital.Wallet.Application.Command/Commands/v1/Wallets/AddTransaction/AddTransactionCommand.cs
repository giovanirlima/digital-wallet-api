using Digital.Wallet.Enums.v1;
using MediatR;

namespace Digital.Wallet.Commands.v1.Wallets.AddTransaction;

public class AddTransactionCommand : IRequest
{
    public int FromUserId { get; set; }
    public int ToUserId { get; set; }
    public TransactionType Transaction { get; set; }
    public decimal Amount { get; set; }
}