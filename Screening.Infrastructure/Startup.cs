using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Screening.Infrastructure.Context;
using Screening.Infrastructure.Models;
using System.Reflection;

namespace Screening.Infrastructure;
public static class Startup
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var assembly = Assembly.GetExecutingAssembly();
        return services
            .AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("MyConnection")))
             .AddMediatR(cfg =>
             {
                 cfg.RegisterServicesFromAssemblies(assembly);
             });

    }

    public static void ConfigureIdentity(this IServiceCollection services)
    {
        services
       .AddIdentity<ApplicationUser, IdentityRole<int>>(options =>
       {
           options.Password.RequireDigit = true;
           options.Password.RequiredLength = 6;
           options.Password.RequireNonAlphanumeric = false;
       })
       .AddEntityFrameworkStores<ApplicationDbContext>()
       .AddDefaultTokenProviders();
    }
}
