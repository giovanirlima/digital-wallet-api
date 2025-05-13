namespace Digital.Wallet.Interfaces.v1;

public interface IAuthService
{
    string GenerateToken(string email);
}