using Appel.SharpTemplate.Domain.Entities;

namespace Appel.SharpTemplate.Domain.Interfaces.Repositories;

public interface IUserRepository : IBaseRepository<UserEntity>
{
    /// <summary>
    /// Asynchronously retrieves a user entity based on a given external ID.
    /// </summary>
    /// <param name="externalId">The external ID of the user to be retrieved. Cannot be null or whitespace.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the UserEntity if found, otherwise null.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when the externalId is null or whitespace.</exception>
    Task<UserEntity?> GetByExternalIdAsync(string? externalId, CancellationToken cancellationToken);

    /// <summary>
    /// Asynchronously retrieves a user entity based on a given email address.
    /// </summary>
    /// <param name="email">The email address of the user to be retrieved. Cannot be null or whitespace.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the UserEntity if found, otherwise null.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when the email is null or whitespace.</exception>
    Task<UserEntity?> GetByEmailAsync(string? email, CancellationToken cancellationToken);
}
