using Appel.SharpTemplate.Application.AppSettings;
using Appel.SharpTemplate.Application.Services;
using Appel.SharpTemplate.Domain.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Appel.SharpTemplate.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        // App Settings
        services.Configure<JwtTokenSettings>(configuration.GetSection("JwtTokenSettings"));
        services.Configure<Argon2HashSettings>(configuration.GetSection("Argon2HashSettings"));

        // Services
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}
