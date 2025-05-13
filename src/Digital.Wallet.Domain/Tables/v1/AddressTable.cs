namespace Digital.Wallet.Tables.v1;

public class AddressTable
{
    public int Id { get; set; }
    public string Street { get; set; } = default!;
    public int Number { get; set; }
    public string City { get; set; } = default!;
    public string Country { get; set; } = default!;
}