namespace Digital.Wallet.Tables.v1;

public class WalletTable
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public UserTable User { get; set; } = default!;
}