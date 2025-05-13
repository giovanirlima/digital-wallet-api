namespace Digital.Wallet.Settings.Models;

public class Authenticator
{
    public string JwtSecret { get; set; } = default!;
    public string JwtIssuer { get; set; } = default!;
    public string JwtAudience { get; set; } = default!;
    public int JwtExpireMinutes { get; set; }
}