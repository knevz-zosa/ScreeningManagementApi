using System.ComponentModel.DataAnnotations;

namespace Screening.Common.Models.Users;
public class UserProfileUpdate
{
    public int Id { get; set; }

    [Required(ErrorMessage = "First name is required")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Last name is required")]
    public string LastName { get; set; }

}

public class UserRoleUpdate
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Role is required.")]
    public string Role { get; set; }
}

public class UserAccessUpdate
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Access is required.")]
    public string Access { get; set; }
}

public class UserPasswordUpdate
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    public string OldPassword { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Confirm Password is required.")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }
}

public class UserUsernameUpdate
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Username is required.")]
    [StringLength(100, MinimumLength = 4, ErrorMessage = "Username must be at least 4 characters long.")]
    public string Username { get; set; }

}

public class UserStatusUpdate
{
    public int Id { get; set; }

    public bool IsActive { get; set; }

}
