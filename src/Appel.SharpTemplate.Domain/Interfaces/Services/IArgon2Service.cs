namespace Appel.SharpTemplate.Domain.Interfaces.Services;

/// <summary>
/// Provides services for hashing passwords and verifying hashed passwords using the Argon2 algorithm.
/// </summary>
public interface IArgon2Service
{
    /// <summary>
    /// Creates a password hash using the Argon2 algorithm.
    /// </summary>
    /// <param name="input">The password to hash. Cannot be null, empty, or whitespace.</param>
    /// <returns>A hashed version of the input string.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the input is null, empty, or whitespace.</exception>
    /// <remarks>
    /// This method generates a secure hash of a given password using the Argon2 algorithm with predefined settings.
    /// The settings include factors like time cost, memory cost, and parallelism to ensure cryptographic strength.
    /// </remarks>
    string CreatePasswordHash(string? input);

    /// <summary>
    /// Verifies a password against a given hash using the Argon2 algorithm.
    /// </summary>
    /// <param name="hash">The hash to verify against. Cannot be null or whitespace.</param>
    /// <param name="input">The password to verify. Cannot be null or whitespace.</param>
    /// <returns>True if the password matches the hash; otherwise, false.</returns>
    /// <remarks>
    /// This method verifies whether a given password matches a specified hash, using the Argon2 algorithm.
    /// It's important for securely validating user credentials without exposing the actual password.
    /// This method returns false if either the hash or the input is null or whitespace.
    /// </remarks>
    bool VerifyPasswordHash(string? hash, string? input);
}
