using Microsoft.EntityFrameworkCore.Query;

namespace Appel.SharpTemplate.Domain.Interfaces.Repositories;

/// <summary>
///     Defines a base repository for managing entities of a specified type.
/// </summary>
/// <typeparam name="TEntity">The type of the entity to be managed by this repository.</typeparam>
public interface IBaseRepository<TEntity> where TEntity : class
{
    /// <summary>
    ///     Asynchronously adds an entity to the data source.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A Task that represents the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown when entity is null.</exception>
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Asynchronously adds a range of entities to the data source.
    /// </summary>
    /// <param name="entities">The entities to add.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A Task that represents the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown when entities is null.</exception>
    Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Deletes an entity from the data source.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    /// <exception cref="ArgumentNullException">Thrown when entity is null.</exception>
    void Delete(TEntity entity);

    /// <summary>
    ///     Asynchronously retrieves an entity by its id from the data source, optionally including related data.
    /// </summary>
    /// <param name="id">The id of the entity to retrieve.</param>
    /// <param name="include">A function to include navigation properties.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains the entity found, or null if no
    ///     entity was found with the provided id.
    /// </returns>
    Task<TEntity?> GetByIdAsync(int id, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Updates an entity in the data source.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <exception cref="ArgumentNullException">Thrown when entity is null.</exception>
    void Update(TEntity entity);

    /// <summary>
    ///     Updates a range of entities in the data source.
    /// </summary>
    /// <param name="entities">The entities to update.</param>
    /// <exception cref="ArgumentNullException">Thrown when entities is null.</exception>
    void UpdateRange(IEnumerable<TEntity> entities);
}
