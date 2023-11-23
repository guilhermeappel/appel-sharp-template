using Appel.SharpTemplate.Domain.Entities;
using Appel.SharpTemplate.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Appel.SharpTemplate.Infrastructure.Data.Repositories;

public sealed class UserRepository : BaseRepository<UserEntity>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context) { }

    public async Task<UserEntity?> GetByExternalIdAsync(string? externalId, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(externalId))
        {
            throw new ArgumentException("External Id cannot be null or whitespace.", nameof(externalId));
        }

        return await DbSet.SingleOrDefaultAsync(x => x.ExternalId.ToString() == externalId, cancellationToken);
    }

    public async Task<UserEntity?> GetByEmailAsync(string? email, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email cannot be null or whitespace.", nameof(email));
        }

        return await DbSet.SingleOrDefaultAsync(x => x.Email!.ToLower() == email.ToLower(), cancellationToken);
    }
}
