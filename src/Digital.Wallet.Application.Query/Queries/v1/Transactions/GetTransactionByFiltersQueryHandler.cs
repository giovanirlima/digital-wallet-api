using AutoMapper;
using Digital.Wallet.DataTransferObjects.v1;
using Digital.Wallet.Interfaces.v1;
using MediatR;

namespace Digital.Wallet.Queries.v1.Transactions;

public class GetTransactionByFiltersQueryHandler(IMapper mapper, ITransactionReadRepository transactionReadRepository) : IRequestHandler<GetTransactionByFiltersQuery, IEnumerable<Transaction>>
{
    private readonly IMapper _mapper = mapper;
    private readonly ITransactionReadRepository _transactionReadRepository = transactionReadRepository;

    public async Task<IEnumerable<Transaction>> Handle(GetTransactionByFiltersQuery request, CancellationToken cancellationToken)
    {
        var transactions = await _transactionReadRepository.GetTransactionByFiltersAsync(request.StartDate, request.EndDate, cancellationToken);

        return _mapper.Map<IEnumerable<Transaction>>(transactions);
    }
}