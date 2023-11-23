using Appel.SharpTemplate.IntegrationTests.Infrastructure;
using Appel.SharpTemplate.TestsCommon.Data;
using FluentAssertions;
using Xunit;

namespace Appel.SharpTemplate.IntegrationTests.Tests.Repositories;

public class UserRepositoryTests : BaseIntegrationTest
{
    [Fact]
    public async Task AddAsync_ShouldAddUser()
    {
        // Arrange
        var userEntity = MockEntityHelper.GetUserEntity();

        // Act
        await UnitOfWork.Users.AddAsync(userEntity);
        await UnitOfWork.SaveAsync();

        // Assert
        var addedEntity = await UnitOfWork.Users.GetByIdAsync(userEntity.Id);
        addedEntity.Should().NotBeNull();
        addedEntity.Should().BeEquivalentTo(userEntity);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldGetUser()
    {
        // Arrange
        var userEntity = MockEntityHelper.GetUserEntity();
        await UnitOfWork.Users.AddAsync(userEntity);
        await UnitOfWork.SaveAsync();

        // Act
        var databaseUserEntity = await UnitOfWork.Users.GetByIdAsync(userEntity.Id);

        // Assert
        databaseUserEntity.Should().NotBeNull();
        databaseUserEntity.Should().BeEquivalentTo(userEntity);
    }
}
