using System.Text;
using Appel.SharpTemplate.Application.AppSettings;
using Appel.SharpTemplate.Common.Constants;
using Appel.SharpTemplate.Domain.Interfaces.Repositories;
using Appel.SharpTemplate.Domain.Interfaces.Services;
using Appel.SharpTemplate.Infrastructure.Data;
using Appel.SharpTemplate.Infrastructure.Data.Repositories;
using Appel.SharpTemplate.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Appel.SharpTemplate.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Api Configuration
        services.AddSwaggerConfiguration();
        services.AddAuthentication(configuration);

        // Repositories
        services.AddDbContext<AppDbContext>(options => options
            .UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
            .UseSnakeCaseNamingConvention());

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddTransient<IUnitOfWork, UnitOfWork>();

        // Services
        services.AddSingleton<IEmailService, EmailService>();
        services.AddSingleton<IArgon2Service, Argon2Service>();
        services.AddSingleton<ITokenService, TokenService>();

        return services;
    }

    public static async Task<IApplicationBuilder> RunMigrationsAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        if (context.Database.IsRelational())
        {
            await context.Database.MigrateAsync();
        }

        return app;
    }

    public static IApplicationBuilder UseSwaggerConfiguration(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.DefaultModelsExpandDepth(-1);
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        });

        return app;
    }

    private static void AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtTokenSettings = configuration.GetSection("JwtTokenSettings").Get<JwtTokenSettings>();
        var secretKey = Encoding.ASCII.GetBytes(jwtTokenSettings?.SecretKey!);

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    RequireExpirationTime = true,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey)
                };
            });
    }

    private static void AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1",
                new OpenApiInfo
                {
                    Title = $"{GeneralConstants.Project.NAME} API",
                    Description = $"{GeneralConstants.Project.NAME}® API Docs",
                    Contact = new OpenApiContact
                    {
                        Name = $"{GeneralConstants.Project.NAME}",
                        Email = GeneralConstants.Project.EMAIL
                    }
                });

            c.AddSecurityDefinition("Bearer",
                new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            });
        });
    }
}
