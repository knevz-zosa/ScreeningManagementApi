using Screening.Common.Models.Auth;
using Screening.Common.Models.Users;
using Screening.Common.Wrapper;
using Screening.Service.Extensions;
using System.Net.Http.Json;

namespace Screening.Service.ApiServices.AuthServices;
public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;

    public AuthService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ResponseWrapper<UserResponse>> GetCurrentUser()
    {
        var response = await _httpClient.GetAsync(AuthEndpoints.CurrentUser);
        return await response.ToResponse<UserResponse>();
    }

    public async Task<ResponseWrapper<UserResponse>> Login(AuthRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync(AuthEndpoints.Login, request);
        return await response.ToResponse<UserResponse>();
    }

    public async Task<ResponseWrapper<UserResponse>> RefreshToken(RefreshTokenRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync(AuthEndpoints.RefreshToken, request);
        return await response.ToResponse<UserResponse>();
    }

    public async Task<ResponseWrapper<string>> RemoveRefreshToken(RefreshTokenRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync(AuthEndpoints.RemoveRefreshToken, request);
        return await response.ToResponse<string>();
    }

}
