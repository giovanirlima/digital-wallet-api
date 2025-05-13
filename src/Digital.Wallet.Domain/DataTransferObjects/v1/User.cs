namespace Digital.Wallet.DataTransferObjects.v1;

public class User
{
    public string Name { get; set; } = default!;
    public DateTime Birthday { get; set; }
    public string Email { get; set; } = default!;
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Address Address { get; set; } = default!;
    public MyWallet? Wallet { get; set; } = default!;
    public IEnumerable<Transaction> SentTransactions { get; set; } = [];
    public IEnumerable<Transaction> ReceivedTransactions { get; set; } = [];
}