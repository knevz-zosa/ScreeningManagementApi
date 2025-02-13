using Screening.Infrastructure.Models;

namespace Screening.Infrastructure.Identity.Jwt;
public interface ITokenService
{
    Task<string> GenerateToken(ApplicationUser user);
    string GenerateRefreshToken();
}
