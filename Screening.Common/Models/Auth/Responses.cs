namespace Screening.Common.Models.Auth;
public class AuthResponse
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public bool IsActive { get; set; }

    public AuthResponse(string accessToken, string refreshToken, string firstName, string lastName, bool isActive)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
        FirstName = firstName;
        LastName = lastName;
        IsActive = isActive;
    }
}

