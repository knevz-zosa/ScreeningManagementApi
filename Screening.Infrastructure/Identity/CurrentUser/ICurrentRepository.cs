using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Screening.Infrastructure.Identity.CurrentUser;
public interface ICurrentRepository
{
    public string? GetUserId();
}

public class CurrentRepository : ICurrentRepository
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public CurrentRepository(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    public string? GetUserId()
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        return userId;
    }
}