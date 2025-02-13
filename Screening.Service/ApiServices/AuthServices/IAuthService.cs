using Screening.Common.Models.Auth;
using Screening.Common.Models.Users;
using Screening.Common.Wrapper;

namespace Screening.Service.ApiServices.AuthServices;
public interface IAuthService
{
    Task<ResponseWrapper<UserResponse>> Login(AuthRequest request);
    Task<ResponseWrapper<UserResponse>> RefreshToken(RefreshTokenRequest request);
    Task<ResponseWrapper<string>> RemoveRefreshToken(RefreshTokenRequest request);
    Task<ResponseWrapper<UserResponse>> GetCurrentUser();
}
