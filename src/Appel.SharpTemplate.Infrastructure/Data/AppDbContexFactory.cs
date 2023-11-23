using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Appel.SharpTemplate.Infrastructure.Data;

/// <summary>
///     AppDbContextFactory is used by Entity Framework Core during design-time tasks, such as migrations.
///     It implements the IDesignTimeDbContextFactory interface to provide a way to create
///     instances of AppDbContext when running migrations and other design-time tasks.
/// </summary>
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    /// <summary>
    ///     This method is called by EF Core to create a new instance of the AppDbContext.
    /// </summary>
    /// <param name="args">Command line arguments passed to the design-time task. Not used in this implementation.</param>
    /// <returns>A new instance of AppDbContext configured with the Npgsql provider and SnakeCaseNamingConvention.</returns>
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseNpgsql().UseSnakeCaseNamingConvention();

        return new AppDbContext(optionsBuilder.Options);
    }
}
