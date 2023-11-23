using Appel.SharpTemplate.Domain.Entities;

namespace Appel.SharpTemplate.Domain.Interfaces.Services;

/// <summary>
/// Provides services for generating JSON Web Tokens (JWT) for user authentication.
/// </summary>
public interface ITokenService
{
    /// <summary>
    /// Generates a JWT token for a given user entity.
    /// </summary>
    /// <param name="userEntity">The user entity for which the token is to be generated. Cannot be null.</param>
    /// <returns>A JWT token string representing the user's authentication token.</returns>
    /// <remarks>
    /// This method creates a JWT token that includes standard claims like email, name, and unique identifier (JTI) of the user.
    /// The token is signed with a symmetric key and has a limited validity period defined by the application's settings.
    /// This token can be used for authenticating and authorizing the user in subsequent requests to the system.
    /// </remarks>
    string GenerateToken(UserEntity userEntity);
}
