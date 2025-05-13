using Digital.Wallet.Enums.v1;
using Digital.Wallet.Events.v1;
using Digital.Wallet.Selectors;
using Digital.Wallet.Tables.v1;
using Microsoft.EntityFrameworkCore;

public class TransactionOperation
{
    public UserTable FromUser { get; private set; }
    public UserTable? ToUser { get; private set; }
    public WalletTable FromWallet { get; private set; }
    public WalletTable? ToWallet { get; private set; }
    public decimal Amount { get; private set; }
    public TransactionType Type { get; private set; }

    private TransactionOperation() { }

    public static async Task<TransactionOperation> Create<T>(T @event, ReadWriteContext context) where T : BaseEvent
    {
        var fromUser = await context.User.Include(x => x.WalletTable)
            .FirstOrDefaultAsync(x => x.Id == @event.FromUserId);

        if (fromUser == null) throw new ArgumentException("Usuário de origem não encontrado");

        var toUser = @event.FromUserId != @event.ToUserId
            ? await context.User.Include(x => x.WalletTable)
                .FirstOrDefaultAsync(x => x.Id == @event.ToUserId)
            : null;

        return new()
        {
            FromUser = fromUser,
            ToUser = toUser,
            FromWallet = fromUser.WalletTable!,
            ToWallet = toUser?.WalletTable,
            Amount = @event.Amount,
            Type = @event.Transaction
        };
    }

    public bool IsValid() =>
        Type switch
        {
            TransactionType.Deposit => true,
            TransactionType.Withdraw => Amount <= FromWallet.Amount,
            TransactionType.Transfer => Amount <= FromWallet.Amount && ToUser != null,
            _ => false
        };

    public void Execute()
    {
        FromWallet.Amount += Type switch
        {
            TransactionType.Deposit => Amount,
            TransactionType.Withdraw => -Amount,
            TransactionType.Transfer => -Amount,
            _ => 0
        };

        if (Type == TransactionType.Transfer && ToWallet != null)
            ToWallet.Amount += Amount;
    }

    public TransactionTable ToTransactionTable() =>
        new()
        {
            FromUserId = FromUser.Id,
            FromWalletId = FromWallet.Id,
            ToUserId = ToUser?.Id ?? FromUser.Id,
            ToWalletId = ToWallet?.Id ?? FromWallet.Id,
            TransactionType = Type,
            Amount = Amount,
            CreatedAt = DateTime.UtcNow
        };
}