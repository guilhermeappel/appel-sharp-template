using System.Diagnostics;
using System.Text.Json;
using Appel.SharpTemplate.Application.AppSettings;
using Appel.SharpTemplate.Application.Mappers;
using Appel.SharpTemplate.Common.Constants;
using Appel.SharpTemplate.Common.ExtensionMethods;
using Appel.SharpTemplate.Domain.Errors;
using Appel.SharpTemplate.Domain.Interfaces.Repositories;
using Appel.SharpTemplate.Domain.Interfaces.Services;
using Appel.SharpTemplate.Domain.Models.User;
using ErrorOr;
using Microsoft.Extensions.Options;

namespace Appel.SharpTemplate.Application.Services;

public class UserService : IUserService
{
    private readonly IArgon2Service _argon2Service;
    private readonly IEmailService _emailService;
    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork _unitOfWork;

    private readonly EmailSettings _emailSettings;

    public UserService(IArgon2Service argon2Service, IEmailService emailService, ITokenService tokenService, IUnitOfWork unitOfWork, IOptions<EmailSettings> emailSettings)
    {
        _argon2Service = argon2Service;
        _emailService = emailService;
        _tokenService = tokenService;
        _unitOfWork = unitOfWork;

        _emailSettings = emailSettings.Value;
    }

    public async Task<ErrorOr<UserAuthenticationModel>> GetByExternalIdAsync(string externalId, CancellationToken cancellationToken = default)
    {
        var userEntity = await _unitOfWork.Users.GetByExternalIdAsync(externalId, cancellationToken);
        if (userEntity is null)
        {
            return UserError.UserNotFound;
        }

        var mapper = new UserMapper();
        return mapper.Map(userEntity);
    }

    public async Task<ErrorOr<UserAuthenticationModel>> LoginAsync(UserLoginModel userLoginModel, CancellationToken cancellationToken = default)
    {
        var userEntity = await _unitOfWork.Users.GetByEmailAsync(userLoginModel.Email, cancellationToken);

        var isTestEnvironment = Environment.GetEnvironmentVariable(GeneralConstants.EnvironmentVariables.ASPNETCORE_ENVIRONMENT) != GeneralConstants.Environments.TEST;
        var isValidHash = isTestEnvironment || _argon2Service.VerifyPasswordHash(userEntity?.Password, userLoginModel.Password);

        if (userEntity is null || !isValidHash)
        {
            return UserError.InvalidCredentials;
        }

        var mapper = new UserMapper();
        var userAuthenticationModel = mapper.Map(userEntity);
        userAuthenticationModel.Token = _tokenService.GenerateToken(userEntity);

        return userAuthenticationModel;
    }

    public async Task<ErrorOr<UserAuthenticationModel>> RegisterAsync(UserRegisterModel userRegisterModel, CancellationToken cancellationToken = default)
    {
        var userEntity = await _unitOfWork.Users.GetByEmailAsync(userRegisterModel.Email, cancellationToken);
        if (userEntity != null)
        {
            return UserError.EmailDuplicated;
        }

        var mapper = new UserMapper();
        userEntity = mapper.Map(userRegisterModel);
        userEntity.ExternalId = Guid.NewGuid();
        userEntity.Password = _argon2Service.CreatePasswordHash(userRegisterModel.Password);

        await _unitOfWork.Users.AddAsync(userEntity, cancellationToken);
        await _unitOfWork.SaveAsync();

        var isTestEnvironment = Environment.GetEnvironmentVariable(GeneralConstants.EnvironmentVariables.ASPNETCORE_ENVIRONMENT) != GeneralConstants.Environments.TEST;
        if (!Debugger.IsAttached && !isTestEnvironment)
        {
            await SendConfirmRegisterEmailAsync(userEntity.Id, userEntity.Email);
        }

        var userAuthenticationModel = mapper.Map(userEntity);
        userAuthenticationModel.Token = _tokenService.GenerateToken(userEntity);

        return userAuthenticationModel;
    }

    private async Task SendConfirmRegisterEmailAsync(int userId, string? email)
    {
        var jsonEmailToken = JsonSerializer.Serialize(new { Email = email, Validity = DateTime.Now.AddHours(24) });
        var emailHash = CryptographyExtensions.Encrypt(_emailSettings.TokenSecretKey, jsonEmailToken);

        var message = await _emailService.LoadEmailTemplateAsync(GeneralConstants.EmailTemplates.USER_EMAIL_CONFIRMATION);
        message = message
            .Replace("{userId}", userId.ToString())
            .Replace("{emailHash}", emailHash);

        await _emailService.SendAsync($"{GeneralConstants.Project.NAME} - Email confirmation", message, email);
    }
}
