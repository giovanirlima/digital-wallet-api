namespace Domain.Interfaces.v1;

public interface IAuthService
{
    string GenerateToken(string email);
}