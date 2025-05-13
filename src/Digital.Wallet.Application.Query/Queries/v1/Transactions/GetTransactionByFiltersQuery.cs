using Digital.Wallet.DataTransferObjects.v1;
using MediatR;

namespace Digital.Wallet.Queries.v1.Transactions;

public class GetTransactionByFiltersQuery : IRequest<IEnumerable<Transaction>>
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}