using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Screening.Common.Models.Users;
using Screening.Common.Wrapper;
using Screening.Infrastructure.Models;

namespace Screening.Infrastructure.Identity.Features.Users.Commands;
public class CreateUserCommand : IRequest<ResponseWrapper<int>>
{
    public UserRequest User { get; set; }
}

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ResponseWrapper<int>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    public CreateUserCommandHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }
    public async Task<ResponseWrapper<int>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {        
        var usersWithSameName = await _userManager.Users
            .Where(u => u.FirstName == request.User.FirstName && u.LastName == request.User.LastName)
            .ToListAsync(cancellationToken);

        if (usersWithSameName.Any())
        {
            return new ResponseWrapper<int>().Failed("Registration failed: A user with the same full name already exists.");
        }

        var user = new ApplicationUser
        {
            UserName = request.User.UserName,
            FirstName = request.User.FirstName,
            LastName = request.User.LastName,
            IsActive = false
        };

        if (request.User.Password.Contains(" "))
        {
            return new ResponseWrapper<int>().Failed("Registration failed: Password cannot contain spaces.");
        }

        var result = await _userManager.CreateAsync(user, request.User.Password);

        if (result.Succeeded)
        {
            return new ResponseWrapper<int>().Success(user.Id, "Registration successful.");
        }

        return new ResponseWrapper<int>().Failed("Registration failed: " + string.Join(", ", result.Errors.Select(e => e.Description)));
    }
}
