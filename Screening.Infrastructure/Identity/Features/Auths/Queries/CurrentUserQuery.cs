using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Screening.Common.Models.Users;
using Screening.Common.Wrapper;
using Screening.Infrastructure.Identity.CurrentUser;
using Screening.Infrastructure.Identity.Jwt;
using Screening.Infrastructure.Models;

namespace Screening.Infrastructure.Identity.Features.Auths.Queries;
public class CurrentUserQuery : IRequest<ResponseWrapper<UserResponse>>
{
}

public class CurrentUseQueryHandler : IRequestHandler<CurrentUserQuery, ResponseWrapper<UserResponse>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;
    private readonly ICurrentRepository _currentRepository;

    public CurrentUseQueryHandler(UserManager<ApplicationUser> userManager, ITokenService tokenService, IMapper mapper, ICurrentRepository currentRepository)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _mapper = mapper;
        _currentRepository = currentRepository;
    }

    public async Task<ResponseWrapper<UserResponse>> Handle(CurrentUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(_currentRepository.GetUserId());
        if (user == null)
        {
            return new ResponseWrapper<UserResponse>().Failed("User not found");
        }

        var roles = await _userManager.GetRolesAsync(user);

        var result = _mapper.Map<UserResponse>(user);

        result.Role = roles.FirstOrDefault() ?? "No Role Assigned";
        return new ResponseWrapper<UserResponse>().Success(result);
    }
}
