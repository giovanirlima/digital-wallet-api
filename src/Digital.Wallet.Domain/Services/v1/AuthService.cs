using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Digital.Wallet.Interfaces.v1;
using Digital.Wallet.Settings;
using Microsoft.IdentityModel.Tokens;

namespace Digital.Wallet.Services.v1;

public sealed class AuthService : IAuthService
{
    public string GenerateToken(string email)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var key = Encoding.ASCII.GetBytes(AppSettings.Authenticator.JwtSecret);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([new(ClaimTypes.Name, email)]),
            Expires = DateTime.UtcNow.AddMinutes(AppSettings.Authenticator.JwtExpireMinutes),
            Issuer = AppSettings.Authenticator.JwtIssuer,
            Audience = AppSettings.Authenticator.JwtAudience,
            SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}