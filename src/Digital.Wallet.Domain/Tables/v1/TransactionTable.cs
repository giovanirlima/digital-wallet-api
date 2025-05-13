using Digital.Wallet.Enums.v1;

namespace Digital.Wallet.Tables.v1;

public class TransactionTable
{
    public int Id { get; set; }
    public int FromUserId { get; set; }
    public UserTable FromUser { get; set; }
    public int FromWalletId { get; set; }
    public WalletTable FromWallet { get; set; }
    public int ToUserId { get; set; }
    public UserTable ToUser { get; set; }
    public int ToWalletId { get; set; }
    public WalletTable ToWallet { get; set; }
    public TransactionType TransactionType { get; set; }
    public decimal Amount { get; set; }
    public DateTime CreatedAt { get; set; }
}