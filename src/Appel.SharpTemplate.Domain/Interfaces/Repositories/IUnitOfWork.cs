namespace Appel.SharpTemplate.Domain.Interfaces.Repositories;

/// <summary>
///     Defines the contract for managing transactions across multiple repositories.
/// </summary>
/// <remarks>
///     An instance of IUnitOfWork symbolizes a session with the database, facilitating querying and saving of entity
///     instances.
///     It encapsulates the Unit of Work and Repository design patterns, streamlining transaction management and data
///     persistence workflows.
///     Each property of this interface representing a repository corresponds to a database table.
///     The IUnitOfWork harmonizes changes across multiple repositories into a single transaction through the SaveAsync
///     method, ensuring data consistency and integrity.
///     This interface is designed to be utilized within a "using" statement, ensuring proper resource disposal upon the
///     completion of the "using" block.
///     The lifecycle of an IUnitOfWork instance should align with the business transaction it orchestrates, such as an
///     HTTP request or a message processing in a message-driven application.
/// </remarks>
public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }

    /// <summary>
    ///     Asynchronously persists the changes made in the context to the underlying database.
    /// </summary>
    /// <returns>The number of state entries written to the database.</returns>
    /// <remarks>
    ///     This method encapsulates the SaveChangesAsync method from AppDbContext to commit the changes to the database.
    /// </remarks>
    /// <exception cref="Microsoft.EntityFrameworkCore.DbUpdateException">
    ///     Thrown when an error occurs while saving to the
    ///     database.
    /// </exception>
    /// <exception cref="Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException">
    ///     Thrown when a concurrency violation occurs
    ///     while saving to the database.
    /// </exception>
    Task<int> SaveAsync();
}
