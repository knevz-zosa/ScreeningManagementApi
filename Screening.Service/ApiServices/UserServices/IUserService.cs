using Microsoft.AspNetCore.Identity;
using Screening.Common.Extensions;
using Screening.Common.Models.Users;
using Screening.Common.Wrapper;

namespace Screening.Service.ApiServices.UserServices;
public interface IUserService
{
    Task<ResponseWrapper<int>> Create(UserRequest request);
    Task<ResponseWrapper<int>> UpdateProfile(UserProfileUpdate update);
    Task<ResponseWrapper<int>> UpdateAccess(UserAccessUpdate update);
    Task<ResponseWrapper<int>> UpdateRole(UserRoleUpdate update);
    Task<ResponseWrapper<int>> UpdateUsername(UserUsernameUpdate update);
    Task<ResponseWrapper<int>> UpdatePassword(UserPasswordUpdate update);
    Task<ResponseWrapper<int>> UpdateStatus(UserStatusUpdate update);
    Task<ResponseWrapper<int>> Delete(int id);
    Task<ResponseWrapper<PagedList<IdentityRole<int>>>> Roles(DataGridQuery dataGridQuery);
    Task<ResponseWrapper<UserResponse>> Get(int id);
    Task<ResponseWrapper<PagedList<UserResponse>>> List(DataGridQuery dataGridQuery);
}
