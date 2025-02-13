using MediatR;
using Microsoft.AspNetCore.Identity;
using Screening.Common.Models.Users;
using Screening.Common.Wrapper;
using Screening.Infrastructure.Models;

namespace Screening.Infrastructure.Identity.Features.Users.Queries;
public class GetUserQuery : IRequest<ResponseWrapper<UserResponse>>
{
    public int Id { get; set; }
}

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, ResponseWrapper<UserResponse>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    public GetUserQueryHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }
    public async Task<ResponseWrapper<UserResponse>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.Id.ToString());
        if (user == null)
        {
            return new ResponseWrapper<UserResponse>().Failed("User not found.");
        }

        var response = new UserResponse
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? "No Role",
            Access = user.Access ?? "No Access",
            IsActive = user.IsActive
        };

        return new ResponseWrapper<UserResponse>().Success(response);
    }
}
