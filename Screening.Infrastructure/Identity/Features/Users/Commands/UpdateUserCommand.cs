using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Screening.Common.Models.Users;
using Screening.Common.Wrapper;
using Screening.Infrastructure.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Screening.Infrastructure.Identity.Features.Users.Commands;

//UPDATE USER PROFILE
public class UpdateUserProfileCommand : IRequest<ResponseWrapper<int>>
{
    public UserProfileUpdate User {  get; set; }
}

public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, ResponseWrapper<int>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    public UpdateUserProfileCommandHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }
    public async Task<ResponseWrapper<int>> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.User.Id.ToString());
        if (user == null)
        {
            return new ResponseWrapper<int>().Failed("User not found.");
        }

        var existingUser = await _userManager.Users
           .Where(u => u.FirstName == request.User.FirstName && u.LastName == request.User.LastName)
           .Where(u => u.Id != user.Id) 
           .FirstOrDefaultAsync(cancellationToken);

        if (existingUser != null)
        {
            return new ResponseWrapper<int>().Failed("A user with the same full name already exists.");
        }

        user.FirstName = request.User.FirstName;
        user.LastName = request.User.LastName;

        var result = await _userManager.UpdateAsync(user);
        if (result.Succeeded)
        {
            return new ResponseWrapper<int>().Success(user.Id, "Profile updated successfully.");
        }

        return new ResponseWrapper<int>().Failed("Failed to update profile: " + string.Join(", ", result.Errors.Select(e => e.Description)));
    }
}

//UPDATE USER USERNAME
public class UpdateUserUsernameCommand : IRequest<ResponseWrapper<int>>
{
    public UserUsernameUpdate User { get; set; }
}

public class UpdateUserUsernameCommandHandler : IRequestHandler<UpdateUserUsernameCommand, ResponseWrapper<int>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    public UpdateUserUsernameCommandHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }
    public async Task<ResponseWrapper<int>> Handle(UpdateUserUsernameCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.User.Id.ToString());
        if (user == null)
        {
            return new ResponseWrapper<int>().Failed("User not found.");
        }

        user.UserName = request.User.Username;

        var result = await _userManager.UpdateAsync(user);
        if (result.Succeeded)
        {
            return new ResponseWrapper<int>().Success(user.Id, "Username updated successfully.");
        }

        return new ResponseWrapper<int>().Failed("Failed to update username: " + string.Join(", ", result.Errors.Select(e => e.Description)));
    }
}

//UPDATE USER PASSWORD
public class UpdateUserPasswordCommand : IRequest<ResponseWrapper<int>>
{
    public UserPasswordUpdate User { get; set; }
}

public class UpdateUserPasswordCommandHandler : IRequestHandler<UpdateUserPasswordCommand, ResponseWrapper<int>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    public UpdateUserPasswordCommandHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }
    public async Task<ResponseWrapper<int>> Handle(UpdateUserPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.User.Id.ToString());
        if (user == null)
        {
            return new ResponseWrapper<int>().Failed("User not found.");
        }

        var result = await _userManager.ChangePasswordAsync(user, request.User.OldPassword, request.User.Password);
        if (result.Succeeded)
        {
            return new ResponseWrapper<int>().Success(user.Id, "Password updated successfully.");
        }

        return new ResponseWrapper<int>().Failed("Failed to update password: " + string.Join(", ", result.Errors.Select(e => e.Description)));
    }
}

//UPDATE USER ROLE
public class UpdateUserRoleCommand : IRequest<ResponseWrapper<int>>
{
    public UserRoleUpdate User { get; set; }
}

public class UpdateUserRoleCommandHandler : IRequestHandler<UpdateUserRoleCommand, ResponseWrapper<int>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    public UpdateUserRoleCommandHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }
    public async Task<ResponseWrapper<int>> Handle(UpdateUserRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.User.Id.ToString());
        if (user == null)
        {
            return new ResponseWrapper<int>().Failed("User not found.");
        }

        var currentRoles = await _userManager.GetRolesAsync(user);
        var result = await _userManager.RemoveFromRolesAsync(user, currentRoles);
        if (!result.Succeeded)
        {
            return new ResponseWrapper<int>().Failed("Failed to remove current roles.");
        }

        result = await _userManager.AddToRoleAsync(user, request.User.Role);
        if (result.Succeeded)
        {
            return new ResponseWrapper<int>().Success(user.Id, "Role updated successfully.");
        }

        return new ResponseWrapper<int>().Failed("Failed to update role: " + string.Join(", ", result.Errors.Select(e => e.Description)));
    }
}

//UPDATE USER ACCESS
public class UpdateUserAccessCommand : IRequest<ResponseWrapper<int>>
{
    public UserAccessUpdate User { get; set; }
}

public class UpdateUserAccessCommandHandler : IRequestHandler<UpdateUserAccessCommand, ResponseWrapper<int>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    public UpdateUserAccessCommandHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }
    public async Task<ResponseWrapper<int>> Handle(UpdateUserAccessCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.User.Id.ToString());
        if (user == null)
        {
            return new ResponseWrapper<int>().Failed("User not found.");
        }

        user.Access = request.User.Access;

        var result = await _userManager.UpdateAsync(user);
        if (result.Succeeded)
        {
            return new ResponseWrapper<int>().Success(user.Id, "Access updated successfully.");
        }

        return new ResponseWrapper<int>().Failed("Failed to update access: " + string.Join(", ", result.Errors.Select(e => e.Description)));
    }
}

//UPDATE USER STATUS
public class UpdateUserStatusCommand : IRequest<ResponseWrapper<int>>
{
    public UserStatusUpdate User { get; set; }
}

public class UpdateUserStatusCommandHandler : IRequestHandler<UpdateUserStatusCommand, ResponseWrapper<int>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    public UpdateUserStatusCommandHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }
    public async Task<ResponseWrapper<int>> Handle(UpdateUserStatusCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.User.Id.ToString());
        if (user == null)
        {
            return new ResponseWrapper<int>().Failed("User not found.");
        }

        user.IsActive = request.User.IsActive;

        var result = await _userManager.UpdateAsync(user);
        if (result.Succeeded)
        {
            return new ResponseWrapper<int>().Success(user.Id, "Status updated successfully.");
        }

        return new ResponseWrapper<int>().Failed("Failed to update status: " + string.Join(", ", result.Errors.Select(e => e.Description)));
    }
}
