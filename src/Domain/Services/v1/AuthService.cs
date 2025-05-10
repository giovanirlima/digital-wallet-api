using CrossCutting.Settings;
using Domain.Interfaces.v1;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Domain.Services.v1;

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