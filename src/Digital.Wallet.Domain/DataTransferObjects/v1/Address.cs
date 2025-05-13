namespace Digital.Wallet.DataTransferObjects.v1;

public class Address
{
    public string Street { get; set; } = default!;
    public int Number { get; set; }
    public string City { get; set; } = default!;
    public string Country { get; set; } = default!;
}