using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Cookbook.SharedData;
using Microsoft.IdentityModel.Tokens;

namespace Cookbook.Core;

public class JwtService(IJwtSettings jwtSettings) : IJwtService
{
    public string GenerateJwt(string id, params string[] roles)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Sub, id),
            new(ClaimTypes.NameIdentifier, id)
        };
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role.Trim())));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            jwtSettings.Issuer,
            jwtSettings.Audience,
            claims,
            expires: DateTime.UtcNow.AddMinutes(jwtSettings.ExpirationMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}