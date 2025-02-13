using System.ComponentModel.DataAnnotations;

namespace Screening.Common.Models.Auth;
public class AuthRequest
{
    [Required(ErrorMessage = "Username is required")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }
}
public class RefreshTokenRequest
{
    public string RefreshToken { get; set; }
}
