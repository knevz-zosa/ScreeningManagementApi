using Microsoft.Extensions.DependencyInjection;
using Screening.Application.Mapping;
using System.Reflection;

namespace Screening.Application;
public static class Startup
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        return services
            .AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(assembly);
            })
             .AddAutoMapper(cfg =>
             {
                 cfg.AddProfile<MappingProfile>();
             }, assembly);
    }
}
