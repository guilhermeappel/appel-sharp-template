using System.Reflection;
using Appel.SharpTemplate.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Appel.SharpTemplate.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<UserEntity> Users => Set<UserEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                modelBuilder
                    .Entity(entityType.ClrType)
                    .HasKey(nameof(BaseEntity.Id));

                modelBuilder
                    .Entity(entityType.ClrType)
                    .Property<DateTimeOffset>(nameof(BaseEntity.CreatedOn))
                    .IsRequired()
                    .HasColumnType("timestamp with time zone");

                modelBuilder
                    .Entity(entityType.ClrType)
                    .Property<DateTimeOffset>(nameof(BaseEntity.LastUpdatedOn))
                    .HasColumnType("timestamp with time zone");
            }
        }

        base.OnModelCreating(modelBuilder);
    }
}
