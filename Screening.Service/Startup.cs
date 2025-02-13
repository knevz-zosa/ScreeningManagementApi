using Microsoft.Extensions.DependencyInjection;
using Screening.Application.IRepositories;
using Screening.Infrastructure.Identity.CurrentUser;
using Screening.Infrastructure.Identity.Jwt;
using Screening.Infrastructure.Repositories;
using Screening.Service.ApiServices._Connect;
using Screening.Service.ApiServices.ApplicantServices;
using Screening.Service.ApiServices.AuthServices;
using Screening.Service.ApiServices.CampusServices;
using Screening.Service.ApiServices.CourceServices;
using Screening.Service.ApiServices.DepartmentServices;
using Screening.Service.ApiServices.ExaminationServices;
using Screening.Service.ApiServices.InterviewServices;
using Screening.Service.ApiServices.RegistrationServices.Undergraduate;
using Screening.Service.ApiServices.ScheduleServices;
using Screening.Service.ApiServices.UserServices;
using Screening.Service.Extensions;

namespace Screening.Service;
public static class Startup
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {        
        services.AddHttpClient();
        return services
            .AddScoped(typeof(IReadRepositoryAsync<,>), typeof(ReadRepositoryAsync<,>))
            .AddScoped(typeof(IWriteRepositoryAsync<,>), typeof(WriteRepositoryAsync<,>))
            .AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>))
             .AddScoped<ICurrentRepository, CurrentRepository>()
            .AddScoped<ITokenService, TokenService>()
            .AddScoped<IConnectService, ConnectService>()
            .AddScoped<IUserService, UserService>()
            .AddScoped<IAuthService, AuthService>()           
            .AddScoped<ICampusService, CampusService>()
            .AddScoped<IDepartmentService, DepartmentService>()
            .AddScoped<ICourseService, CourseService>()
            .AddScoped<IScheduleService, ScheduleService>()
            .AddScoped<IExaminationService, ExaminationService>()
            .AddScoped<IInterviewService, InterviewService>()
            .AddScoped<IApplicantService, ApplicantService>()
            .AddScoped<IUndergraduateRegistrationService, UndergraduateRegistrationService>()
            .AddAutoMapper(cfg =>
            {
                cfg.AddProfile<UserMapper>();
            }, typeof(UserMapper).Assembly);
            
    }

}

