using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Screening.Common.Models.Auth;
using Screening.Common.Wrapper;
using Screening.Infrastructure.Models;
using System.Security.Cryptography;
using System.Text;

namespace Screening.Infrastructure.Identity.Features.Auths.Queries;
public class RemoveRefreshTokenQuery : IRequest<ResponseWrapper<string>>
{
    public RefreshTokenRequest Get { get; set; }
}

public class RemoveRefreshTokenQueryHandler : IRequestHandler<RemoveRefreshTokenQuery, ResponseWrapper<string>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    public RemoveRefreshTokenQueryHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<ResponseWrapper<string>> Handle(RemoveRefreshTokenQuery request, CancellationToken cancellationToken)
    {
       
        try
        {
            using var sha256 = SHA256.Create();

            var refreshTokenHash = sha256.ComputeHash(Encoding.UTF8.GetBytes(request.Get.RefreshToken));
            var hashedRefreshToken = Convert.ToBase64String(refreshTokenHash);

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == hashedRefreshToken);
            if (user == null)
            {
                throw new Exception("Invalid refresh token");
            }

            if (user.RefreshTokenExpiryTime < DateTime.Now)
            {
                return new ResponseWrapper<string>().Failed($"Refresh token expired for User Id: {user.Id}");
            }

            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return new ResponseWrapper<string>().Failed("Failed to update user");
            }
            return new ResponseWrapper<string>().Success("Successfully removed refresh token");
        }
        catch (Exception ex)
        {
            return new ResponseWrapper<string>().Failed("Failed to remove refresh token");
        }
    }
}
