using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Screening.Infrastructure.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Screening.Infrastructure.Identity.Jwt;
public class TokenService : ITokenService
{
    private readonly SymmetricSecurityKey _key;
    private readonly string? _validIssuer;
    private readonly string? _validAudience;
    private readonly double _expires;
    private readonly UserManager<ApplicationUser> _userManager;
    public TokenService(IConfiguration configuration, UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;

        var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();
        if (jwtSettings == null || string.IsNullOrEmpty(jwtSettings.Key))
        {
            throw new InvalidOperationException("JWT key is not configured");
        }
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));
        _validIssuer = jwtSettings.ValidIssuer;
        _validAudience = jwtSettings.ValidAudience;
        _expires = jwtSettings.Expires;
    }

    public async Task<string> GenerateToken(ApplicationUser user)
    {
        var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);
        var claims = await GetClaims(user);
        var tokenOptions = GenerateTokenOptions(credentials, claims);
        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }

    private async Task<List<Claim>> GetClaims(ApplicationUser user)
    {

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user?.UserName ?? string.Empty),
            new Claim("FirstName", user.FirstName),
            new Claim("LastName", user.LastName)
        };
        var Roles = await _userManager.GetRolesAsync(user);
        claims.AddRange(Roles.Select(role => new Claim(ClaimTypes.Role, role)));
        return claims;
    }

    private JwtSecurityToken GenerateTokenOptions(SigningCredentials credentials, List<Claim> claims)
    {
        return new JwtSecurityToken(
            issuer: _validIssuer,
            audience: _validAudience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(_expires),
            signingCredentials: credentials
            );
    }
    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);

        var refreshToken = Convert.ToBase64String(randomNumber);
        return refreshToken;
    }
}
