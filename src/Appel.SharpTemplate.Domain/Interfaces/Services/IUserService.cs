using Appel.SharpTemplate.Domain.Models.User;
using ErrorOr;

namespace Appel.SharpTemplate.Domain.Interfaces.Services;

public interface IUserService
{
    Task<ErrorOr<UserAuthenticationModel>> GetByExternalIdAsync(string externalId, CancellationToken cancellationToken = default);

    Task<ErrorOr<UserAuthenticationModel>> LoginAsync(UserLoginModel userLoginModel, CancellationToken cancellationToken = default);

    Task<ErrorOr<UserAuthenticationModel>> RegisterAsync(UserRegisterModel userRegisterModel, CancellationToken cancellationToken = default);
}
