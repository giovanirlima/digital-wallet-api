using Digital.Wallet.Enums.v1;

namespace Digital.Wallet.DataTransferObjects.v1;

public class Transaction
{
    public string FromUsername { get; set; } = default!;
    public int FromUserId { get; set; }
    public string ToUsername { get; set; } = default!;
    public int ToUserId { get; set; }
    public TransactionType TransactionType { get; set; }
    public decimal Amount { get; set; }
    public DateTime CreatedAt { get; set; }
}