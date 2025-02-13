using Microsoft.OpenApi.Models;
using Screening.Api.Exception;
using Screening.Api.Extensions;
using Screening.Infrastructure;
using Screening.Infrastructure.Context;
using Screening.Infrastructure.Seeds;
using Screening.Application;
using Screening.Service;
namespace Screening.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();
        builder.Services.ConfigureIdentity();
        builder.Services.ConfigureJwt(builder.Configuration);
        builder.Services.AddDatabase(builder.Configuration);
        builder.Services.AddRepositories();
        builder.Services.AddApplicationServices();
        
        
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddTransient<RoleSeeder>();
        builder.Services.AddTransient<UserSeeder>();
        
        builder.Services.AddAuthorization();
        builder.Services.AddCors();

        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "User Auth", Version = "v1", Description = "Services to Authenticate user" });            

            // Add JWT Bearer security definition to Swagger UI
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Please enter a valid token." 
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "Auth",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                    new List<string> { }
                }
            });
        });
        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Ensure database is created, but only if not in a test environment
            if (!app.Environment.IsEnvironment("Testing"))
            {
                context.Database.EnsureCreated();
                var roleSeeder = scope.ServiceProvider.GetRequiredService<RoleSeeder>();
                roleSeeder.SeedRolesAsync().Wait();

                var userSeeder = scope.ServiceProvider.GetRequiredService<UserSeeder>();
                userSeeder.SeedUserAsync().Wait();
            }
        }

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseCors(x => x
               .AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());
        app.UseHttpsRedirection();
        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}


