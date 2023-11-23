using Appel.SharpTemplate.Application.AppSettings;
using Appel.SharpTemplate.Application.Services;
using Appel.SharpTemplate.Common.Constants;
using Appel.SharpTemplate.Domain.Entities;
using Appel.SharpTemplate.Domain.Errors;
using Appel.SharpTemplate.Domain.Interfaces.Repositories;
using Appel.SharpTemplate.Domain.Interfaces.Services;
using Appel.SharpTemplate.Domain.Models.User;
using Appel.SharpTemplate.TestsCommon.Data;
using Appel.SharpTemplate.UnitTests.Data;
using ErrorOr;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Appel.SharpTemplate.UnitTests.Tests.Services;

public class UserServiceTests
{
    private readonly Mock<IArgon2Service> _mockArgon2Service = new();
    private readonly Mock<IEmailService> _mockEmailService = new();
    private readonly Mock<ITokenService> _mockTokenService = new();
    private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();

    private readonly UserService _userService;

    public UserServiceTests()
    {
        Environment.SetEnvironmentVariable(GeneralConstants.EnvironmentVariables.ASPNETCORE_ENVIRONMENT, GeneralConstants.Environments.TEST);

        var mockEmailSettings = Mock.Of<IOptions<EmailSettings>>(options => options.Value == MockSettingsHelper.GetEmailSettings());

        _userService = new UserService(
            _mockArgon2Service.Object,
            _mockEmailService.Object,
            _mockTokenService.Object,
            _mockUnitOfWork.Object,
            mockEmailSettings);
    }

    [Fact]
    public async Task GetByExternalIdAsync_UserFound_ReturnsUserAuthenticationModel()
    {
        // Arrange
        var externalId = Guid.NewGuid();
        var userEntity = MockEntityHelper.GetUserEntity(externalId);

        _mockUnitOfWork
            .Setup(uow => uow.Users.GetByExternalIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(userEntity);

        // Act
        var result = await _userService.GetByExternalIdAsync(externalId.ToString());

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ErrorOr<UserAuthenticationModel>>();

        _mockUnitOfWork.Verify(uow => uow.Users.GetByExternalIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByExternalIdAsync_UserNotFound_ReturnsUserNotFoundError()
    {
        // Arrange
        _mockUnitOfWork
            .Setup(uow => uow.Users.GetByExternalIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(default(UserEntity?));

        // Act
        var result = await _userService.GetByExternalIdAsync(It.IsAny<string>());

        // Assert
        result.Should().NotBeNull();
        result.IsError.Should().BeTrue();
        result.Errors.Should().ContainSingle(error => error == UserError.UserNotFound);

        _mockUnitOfWork.Verify(uow => uow.Users.GetByExternalIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task LoginAsync_ValidCredentials_ReturnsUserAuthenticationModel()
    {
        // Arrange
        const string EXPECTED_TOKEN_TO_RETURN = "MOCK_TOKEN";

        var userLoginModel = MockModelHelper.GetUserLoginModel();
        var userEntity = MockEntityHelper.GetUserEntity();

        _mockUnitOfWork
            .Setup(uow => uow.Users.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(userEntity);

        _mockArgon2Service
            .Setup(service => service.VerifyPasswordHash(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(true);

        _mockTokenService
            .Setup(service => service.GenerateToken(It.IsAny<UserEntity>()))
            .Returns(EXPECTED_TOKEN_TO_RETURN);

        // Act
        var result = await _userService.LoginAsync(userLoginModel);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ErrorOr<UserAuthenticationModel>>();
        var userAuthenticationModel = result.Match(success => success, error => default!);

        userAuthenticationModel.Should().NotBeNull();
        userAuthenticationModel.Token.Should().Be(EXPECTED_TOKEN_TO_RETURN);

        _mockUnitOfWork.Verify(uow => uow.Users.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        _mockArgon2Service.Verify(service => service.VerifyPasswordHash(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        _mockTokenService.Verify(service => service.GenerateToken(It.IsAny<UserEntity>()), Times.Once);
    }

    [Fact]
    public async Task LoginAsync_UserNotExist_ReturnsInvalidCredentialsError()
    {
        // Arrange
        var userLoginModel = MockModelHelper.GetUserLoginModel();

        _mockUnitOfWork
            .Setup(uow => uow.Users.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(default(UserEntity?));

        _mockArgon2Service
            .Setup(service => service.VerifyPasswordHash(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(true);

        // Act
        var result = await _userService.LoginAsync(userLoginModel);

        // Assert
        result.Should().NotBeNull();
        result.IsError.Should().BeTrue();
        result.Errors.Should().ContainSingle(error => error == UserError.InvalidCredentials);

        _mockUnitOfWork.Verify(uow => uow.Users.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        _mockArgon2Service.Verify(service => service.VerifyPasswordHash(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        _mockTokenService.Verify(service => service.GenerateToken(It.IsAny<UserEntity>()), Times.Never);
    }

    [Fact]
    public async Task LoginAsync_InvalidPasswordHash_ReturnsInvalidCredentialsError()
    {
        // Arrange
        var userLoginModel = MockModelHelper.GetUserLoginModel();
        var userEntity = MockEntityHelper.GetUserEntity();

        _mockUnitOfWork
            .Setup(uow => uow.Users.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(userEntity);

        _mockArgon2Service
            .Setup(service => service.VerifyPasswordHash(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(false);

        // Act
        var result = await _userService.LoginAsync(userLoginModel);

        // Assert
        result.Should().NotBeNull();
        result.IsError.Should().BeTrue();
        result.Errors.Should().ContainSingle(error => error == UserError.InvalidCredentials);

        _mockUnitOfWork.Verify(uow => uow.Users.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        _mockArgon2Service.Verify(service => service.VerifyPasswordHash(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        _mockTokenService.Verify(service => service.GenerateToken(It.IsAny<UserEntity>()), Times.Never);
    }

    [Fact]
    public async Task RegisterAsync_NewEmail_ReturnsUserAuthenticationModel()
    {
        // Arrange
        const string EXPECTED_TOKEN_TO_RETURN = "MOCK_TOKEN";
        var userRegisterModel = MockModelHelper.GetUserRegisterModel();

        _mockArgon2Service
            .Setup(service => service.CreatePasswordHash(It.IsAny<string>()))
            .Returns(It.IsAny<string>());

        _mockUnitOfWork
            .Setup(uow => uow.Users.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(default(UserEntity?));

        _mockTokenService
            .Setup(service => service.GenerateToken(It.IsAny<UserEntity>()))
            .Returns(EXPECTED_TOKEN_TO_RETURN);

        _mockEmailService
            .Setup(service => service.LoadEmailTemplateAsync(It.IsAny<string>()))
            .ReturnsAsync("MOCK_EMAIL_MESSAGE");

        _mockEmailService
            .Setup(service => service.SendAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _userService.RegisterAsync(userRegisterModel);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ErrorOr<UserAuthenticationModel>>();

        var userAuthenticationModel = result.Match(success => success, error => default!);
        userAuthenticationModel.Should().NotBeNull();
        userAuthenticationModel.Token.Should().Be(EXPECTED_TOKEN_TO_RETURN);

        _mockArgon2Service.Verify(service => service.CreatePasswordHash(It.IsAny<string>()), Times.Once);

        _mockUnitOfWork.Verify(uow => uow.Users.AddAsync(It.IsAny<UserEntity>(), It.IsAny<CancellationToken>()), Times.Once);
        _mockUnitOfWork.Verify(uow => uow.SaveAsync(), Times.Once);

        _mockEmailService.Verify(service => service.SendAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        _mockTokenService.Verify(service => service.GenerateToken(It.IsAny<UserEntity>()), Times.Once);
    }

    [Fact]
    public async Task RegisterAsync_EmailAlreadyExists_ReturnsEmailDuplicatedError()
    {
        // Arrange
        var userEntity = MockEntityHelper.GetUserEntity();
        var userModel = MockModelHelper.GetUserRegisterModel();

        _mockUnitOfWork
            .Setup(uow => uow.Users.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(userEntity);

        // Act
        var result = await _userService.RegisterAsync(userModel);

        // Assert
        result.Should().NotBeNull();
        result.IsError.Should().BeTrue();
        result.Errors.Should().ContainSingle(error => error == UserError.EmailDuplicated);

        _mockArgon2Service.Verify(service => service.CreatePasswordHash(It.IsAny<string>()), Times.Never);
        _mockUnitOfWork.Verify(uow => uow.Users.AddAsync(It.IsAny<UserEntity>(), It.IsAny<CancellationToken>()), Times.Never);
        _mockUnitOfWork.Verify(uow => uow.SaveAsync(), Times.Never);
        _mockEmailService.Verify(service => service.SendAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        _mockTokenService.Verify(service => service.GenerateToken(It.IsAny<UserEntity>()), Times.Never);
    }
}
