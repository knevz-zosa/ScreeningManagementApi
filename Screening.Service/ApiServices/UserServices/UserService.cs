using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Screening.Common.Extensions;
using Screening.Common.Models.Users;
using Screening.Common.Wrapper;
using Screening.Service.Extensions;
using System.Net.Http.Json;

namespace Screening.Service.ApiServices.UserServices;
public class UserService : IUserService
{
    
    private readonly HttpClient _httpClient;
    public UserService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ResponseWrapper<int>> Create(UserRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync(UserEndpoints.Create, request);
        return await response.ToResponse<int>();
    }

    public async Task<ResponseWrapper<int>> Delete(int id)
    {
        var response = await _httpClient.DeleteAsync($"{UserEndpoints.Delete}/{id}");
        return await response.ToResponse<int>();
    }

    public async Task<ResponseWrapper<UserResponse>> Get(int id)
    {
        var response = await _httpClient.GetAsync(UserEndpoints.Get(id));
        return await response.ToResponse<UserResponse>();
    }  

    public async Task<ResponseWrapper<int>> UpdateAccess(UserAccessUpdate update)
    {
        var response = await _httpClient.PutAsJsonAsync(UserEndpoints.UpdateAccess, update);
        return await response.ToResponse<int>();
    }

    public async Task<ResponseWrapper<int>> UpdatePassword(UserPasswordUpdate update)
    {
        var response = await _httpClient.PutAsJsonAsync(UserEndpoints.UpdatePassword, update);
        return await response.ToResponse<int>();
    }

    public async Task<ResponseWrapper<int>> UpdateProfile(UserProfileUpdate update)
    {
        var response = await _httpClient.PutAsJsonAsync(UserEndpoints.UpdateProfile, update);
        return await response.ToResponse<int>();
    }

    public async Task<ResponseWrapper<int>> UpdateRole(UserRoleUpdate update)
    {
        var response = await _httpClient.PutAsJsonAsync(UserEndpoints.UpdateRole, update);
        return await response.ToResponse<int>();
    }

    public async Task<ResponseWrapper<int>> UpdateUsername(UserUsernameUpdate update)
    {
        var response = await _httpClient.PutAsJsonAsync(UserEndpoints.UpdateUsername, update);
        return await response.ToResponse<int>();
    }
    public async Task<ResponseWrapper<int>> UpdateStatus(UserStatusUpdate update)
    {
        var response = await _httpClient.PutAsJsonAsync(UserEndpoints.UpdateIsActive, update);
        return await response.ToResponse<int>();
    }

    public async Task<ResponseWrapper<PagedList<IdentityRole<int>>>> Roles(DataGridQuery dataGridQuery)
    {
        var response = await _httpClient.GetAsync(UserEndpoints.Roles);
        return await response.ToResponse<PagedList<IdentityRole<int>>>();
    }

    public async Task<ResponseWrapper<PagedList<UserResponse>>> List(DataGridQuery dataGridQuery)
    {       
        var queryParams = new Dictionary<string, string>();

        if (!string.IsNullOrWhiteSpace(dataGridQuery.Search))
            queryParams["search"] = dataGridQuery.Search;

        if (!string.IsNullOrWhiteSpace(dataGridQuery.SchoolYear))
            queryParams["schoolYear"] = dataGridQuery.SchoolYear;

        if (dataGridQuery.Page.HasValue)
            queryParams["page"] = dataGridQuery.Page.Value.ToString();

        if (dataGridQuery.PageSize.HasValue)
            queryParams["pageSize"] = dataGridQuery.PageSize.Value.ToString();

        if (!string.IsNullOrWhiteSpace(dataGridQuery.SortField))
            queryParams["sortField"] = dataGridQuery.SortField;

        queryParams["sortDir"] = dataGridQuery.SortDir.ToString().ToLower();

        // Construct the final URL with query parameters
        var url = QueryHelpers.AddQueryString(UserEndpoints.List, queryParams);

        var response = await _httpClient.GetAsync(url);
        return await response.ToResponse<PagedList<UserResponse>>();
    }
}
