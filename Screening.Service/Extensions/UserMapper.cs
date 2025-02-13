using AutoMapper;
using Screening.Common.Models.Users;
using Screening.Infrastructure.Models;

namespace Screening.Service.Extensions;
public class UserMapper : Profile
{
    public UserMapper()
    {
        CreateMap<ApplicationUser, UserResponse>();
        CreateMap<ApplicationUser, UserProfileUpdate>();
        CreateMap<ApplicationUser, UserRoleUpdate>();
        CreateMap<ApplicationUser, UserAccessUpdate>();
        CreateMap<ApplicationUser, UserPasswordUpdate>();
        CreateMap<ApplicationUser, UserUsernameUpdate>();
        CreateMap<ApplicationUser, UserLoginResponse>();

        CreateMap<UserResponse, UserRoleUpdate>();
    }
}
