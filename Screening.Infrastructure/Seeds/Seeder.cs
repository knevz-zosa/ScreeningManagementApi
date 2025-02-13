using Microsoft.AspNetCore.Identity;
using Screening.Infrastructure.Models;

namespace Screening.Infrastructure.Seeds;
public class RoleSeeder
{
    private readonly RoleManager<IdentityRole<int>> _roleManager;

    public RoleSeeder(RoleManager<IdentityRole<int>> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task SeedRolesAsync()
    {
        // List of roles to seed
        var roles = new List<string> { "Administrator", "Manager", "Registrar" };

        foreach (var role in roles)
        {
            var roleExist = await _roleManager.RoleExistsAsync(role);
            if (!roleExist)
            {
                // Create the role if it doesn't exist
                await _roleManager.CreateAsync(new IdentityRole<int>(role));
            }
        }
    }
}

public class UserSeeder
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole<int>> _roleManager;

    public UserSeeder(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<int>> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task SeedUserAsync()
    {
        // Check if the user exists
        var user = await _userManager.FindByNameAsync("admin123");
        if (user == null)
        {
            user = new ApplicationUser
            {
                UserName = "admin123",
                FirstName = "John",
                LastName = "Doe",
                IsActive = true,
                Access = "All"
            };

            // Create the user with a password
            var result = await _userManager.CreateAsync(user, "!P@ssw0rd");

            if (result.Succeeded)
            {
                // Ensure the Administrator role exists and assign it to the user
                var roleExists = await _roleManager.RoleExistsAsync("Administrator");
                if (!roleExists)
                {
                    await _roleManager.CreateAsync(new IdentityRole<int>("Administrator"));
                }

                await _userManager.AddToRoleAsync(user, "Administrator");
            }
            else
            {
                // Handle error or log failure
                throw new Exception("Failed to create user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }
    }
}
