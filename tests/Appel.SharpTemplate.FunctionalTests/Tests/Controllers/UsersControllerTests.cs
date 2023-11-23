using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Appel.SharpTemplate.Common.Constants;
using Appel.SharpTemplate.Common.ExtensionMethods;
using Appel.SharpTemplate.Domain.Models.User;
using Appel.SharpTemplate.FunctionalTests.Data;
using Appel.SharpTemplate.FunctionalTests.Infrastructure;
using Appel.SharpTemplate.TestsCommon.Constants;
using FluentAssertions;
using Xunit;

namespace Appel.SharpTemplate.FunctionalTests.Tests.Controllers;

public class UsersControllerTests : BaseControllerTests
{
    public UsersControllerTests(CustomWebApplicationFactory<Program> factory) : base(factory) { }

    [Fact]
    public async Task LoginAsync_ValidCredentials_ReturnsOk()
    {
        // Arrange
        var userLoginContract = MockContractHelper.GetUserLoginContract(UserTestConstants.Entity.User1.EMAIL, UserTestConstants.Entity.User1.PASSWORD);
        var stringContent = new StringContent(JsonSerializer.Serialize(userLoginContract), Encoding.UTF8, GeneralConstants.MediaTypes.APPLICATION_JSON);

        // Act
        var response = await Client.PostAsync("/api/users/login", stringContent);

        // Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var userAuthModel = (await response.Content.ReadAsStringAsync()).DeserializeCaseInsensitive<UserAuthenticationModel>();
        userAuthModel.Should().NotBeNull();
        userAuthModel?.Token.Should().NotBeNullOrEmpty();
        userAuthModel?.Email.Should().Be(UserTestConstants.Entity.User1.EMAIL);
    }

    [Fact]
    public async Task LoginAsync_InvalidCredentials_ReturnsConflict()
    {
        // Arrange
        var userLoginContract = MockContractHelper.GetUserLoginContract("invalid@email.com", "wrongPassword");
        var stringContent = new StringContent(JsonSerializer.Serialize(userLoginContract), Encoding.UTF8, GeneralConstants.MediaTypes.APPLICATION_JSON);

        // Act
        var response = await Client.PostAsync("/api/users/login", stringContent);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }


    [Fact]
    public async Task RegisterAsync_ValidData_ReturnsCreated()
    {
        // Arrange
        var userRegisterContract = MockContractHelper.GetUserRegisterContract(UserTestConstants.Entity.NewUser.EMAIL, UserTestConstants.Entity.NewUser.PASSWORD);
        var stringContent = new StringContent(JsonSerializer.Serialize(userRegisterContract), Encoding.UTF8, GeneralConstants.MediaTypes.APPLICATION_JSON);

        // Act
        var response = await Client.PostAsync("/api/users/register", stringContent);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var userAuthModel = (await response.Content.ReadAsStringAsync()).DeserializeCaseInsensitive<UserAuthenticationModel>();
        userAuthModel.Should().NotBeNull();
        userAuthModel?.Token.Should().NotBeNullOrEmpty();
        userAuthModel?.Email.Should().Be(UserTestConstants.Entity.NewUser.EMAIL);
    }

    [Fact]
    public async Task GetByExternalIdAsync_ExistingUser_ReturnsOk()
    {
        // Arrange
        await AuthenticateUserAsync(UserTestConstants.Entity.User1.EMAIL, UserTestConstants.Entity.User1.PASSWORD);

        // Act
        var response = await Client.GetAsync($"/api/users/{UserTestConstants.Entity.User1.EXTERNAL_ID}");

        // Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var userAuthModel = (await response.Content.ReadAsStringAsync()).DeserializeCaseInsensitive<UserAuthenticationModel>();
        userAuthModel.Should().NotBeNull();
        userAuthModel?.Email.Should().Be(UserTestConstants.Entity.User1.EMAIL);
    }

    [Fact]
    public async Task GetByExternalIdAsync_NonExistingUser_ReturnsNotFound()
    {
        // Arrange
        await AuthenticateUserAsync(UserTestConstants.Entity.User1.EMAIL, UserTestConstants.Entity.User1.PASSWORD);

        // Act
        var response = await Client.GetAsync($"/api/users/non-existing-id");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetByExternalIdAsync_UnauthorizedUser_ReturnsUnauthorized()
    {
        // Arrange
        var response = await Client.GetAsync($"/api/users/{UserTestConstants.Entity.User1.EXTERNAL_ID}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    private async Task AuthenticateUserAsync(string email, string password)
    {
        var userLoginContract = MockContractHelper.GetUserLoginContract(email, password);
        var stringContent = new StringContent(JsonSerializer.Serialize(userLoginContract), Encoding.UTF8, GeneralConstants.MediaTypes.APPLICATION_JSON);

        var userLoginResponseMessage = await Client.PostAsync("/api/users/login", stringContent);
        var userLoginResponseMessageString = await userLoginResponseMessage.Content.ReadAsStringAsync();
        var userAuthenticationModel = userLoginResponseMessageString.DeserializeCaseInsensitive<UserAuthenticationModel>();

        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userAuthenticationModel?.Token);
    }
}
