using Microsoft.AspNetCore.Identity;
using Screening.Domain.Entities;

namespace Screening.Infrastructure.Models;

public class ApplicationUser : IdentityUser<int>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Access { get; set; }
    public bool IsActive { get; set; } = false;
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
    public List<Transfer>? Transfers { get; set; } = new List<Transfer>();
}
