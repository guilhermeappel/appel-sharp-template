using Appel.SharpTemplate.Infrastructure.Data;
using Appel.SharpTemplate.TestsCommon.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Appel.SharpTemplate.FunctionalTests.Infrastructure;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            builder.ConfigureAppConfiguration((_, conf) =>
            {
                conf.AddJsonFile("appsettings.Test.json");
            });

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDatabase");
            });

            var serviceProvider = services.BuildServiceProvider();
            using (var scope = serviceProvider.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var appDbContext = scopedServices.GetRequiredService<AppDbContext>();

                appDbContext.Database.EnsureCreated();

                try
                {
                    DatabaseSetup.SeedData(appDbContext);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "An error occurred seeding the database.");
                }
            }
        });
    }
}
