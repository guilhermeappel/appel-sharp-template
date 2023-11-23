using Appel.SharpTemplate.Domain.Entities;
using Appel.SharpTemplate.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Appel.SharpTemplate.Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public IUserRepository Users { get; }

    public UnitOfWork(AppDbContext context, IUserRepository userRepository)
    {
        _context = context;
        Users = userRepository;
    }

    public async Task<int> SaveAsync()
    {
        await _semaphore.WaitAsync();

        try
        {
            var entries = _context.ChangeTracker.Entries();

            foreach (var entry in entries)
            {
                if (entry.Entity is BaseEntity trackable)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            trackable.CreatedOn = DateTimeOffset.UtcNow;
                            break;
                        case EntityState.Modified:
                            trackable.LastUpdatedOn = DateTimeOffset.UtcNow;
                            break;
                    }
                }
            }

            return await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred in SaveAsync");
            throw;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _context.Dispose();
        }
    }
}
