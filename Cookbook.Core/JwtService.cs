using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Cookbook.Core;

public class JwtService(IConfiguration configuration) : IJwtService
{
    private readonly string _audience = configuration["JwtAudience"]!;
    private readonly int _expirationMinutes = int.Parse(configuration["JwtExpirationMinutes"]!);
    private readonly string _issuer = configuration["JwtIssuer"]!;
    private readonly string _secret = configuration["JwtSecret"]!;

    public string GenerateJwt(string id, params string[] roles)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Sub, id),
            new(ClaimTypes.NameIdentifier, id)
        };
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role.Trim())));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _issuer,
            _audience,
            claims,
            expires: DateTime.UtcNow.AddMinutes(_expirationMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}