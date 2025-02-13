using MediatR;
using Microsoft.AspNetCore.Identity;
using Screening.Common.Wrapper;
using Screening.Infrastructure.Models;

namespace Screening.Infrastructure.Identity.Features.Users.Commands;
public class DeleteUserCommand : IRequest<ResponseWrapper<int>>
{
    public int Id { get; set; }
}

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, ResponseWrapper<int>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    public DeleteUserCommandHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }
    public async Task<ResponseWrapper<int>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.Id.ToString());
        if (user == null)
        {
            return new ResponseWrapper<int>().Failed("User not found.");
        }

        var result = await _userManager.DeleteAsync(user);
        if (result.Succeeded)
        {
            return new ResponseWrapper<int>().Success(request.Id, "User deleted successfully.");
        }

        return new ResponseWrapper<int>().Failed("Failed to delete user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
    }
}
