namespace Digital.Wallet.Tables.v1;

public class UserTable
{
    public int Id { get; set; }
    public int WalletId { get; set; }
    public int AddressId { get; set; }
    public string Name { get; set; } = default!;
    public string Password { get; set; } = default!;
    public DateTime Birthday { get; set; }
    public string Email { get; set; } = default!;
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public AddressTable? AddressTable { get; set; }
    public WalletTable? WalletTable { get; set; }
    public ICollection<TransactionTable> SentTransaction { get; set; } = [];
    public ICollection<TransactionTable> ReceivedTransaction { get; set; } = [];
}