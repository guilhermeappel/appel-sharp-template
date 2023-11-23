using Appel.SharpTemplate.Application;
using Appel.SharpTemplate.Common.Utilities;
using Appel.SharpTemplate.Infrastructure;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

try
{
    builder.Services
        .AddApplication(builder.Configuration)
        .AddInfrastructure(builder.Configuration);

    builder.Services.AddControllers(options =>
    {
        options.Conventions
            .Add(new RouteTokenTransformerConvention(new KebabParameterTransformer()));
    });

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSingleton<ProblemDetailsFactory, CustomProblemDetailsFactory>();

    var app = builder.Build();

    app.UseSwaggerConfiguration();

    app.UseExceptionHandler("/errors");
    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    await app.RunMigrationsAsync();
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application start-up failed");
}
finally
{
    Log.CloseAndFlush();
}

// Make the implicit Program.cs class public, so integration tests can reference the correct assembly for host building
public partial class Program { }
