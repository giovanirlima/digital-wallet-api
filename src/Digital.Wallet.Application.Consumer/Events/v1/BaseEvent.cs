using Digital.Wallet.Enums.v1;

namespace Digital.Wallet.Events.v1;

public class BaseEvent
{
    public int FromUserId { get; set; }
    public TransactionType Transaction { get; set; }
    public decimal Amount { get; set; }
    public int ToUserId { get; set; }
}