using Domain.Enums.v1;
using MediatR;

namespace Infrastructure.Data.Command.Commands.v1.Wallets.AddTransaction;

public class AddTransactionCommand : IRequest
{
    public int FromUserId { get; set; }
    public int ToUserId { get; set; }
    public TransactionType Transaction { get; set; }
    public decimal Amount { get; set; }
}