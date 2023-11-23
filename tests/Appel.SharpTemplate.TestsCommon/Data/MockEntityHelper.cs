using Appel.SharpTemplate.Domain.Entities;

namespace Appel.SharpTemplate.TestsCommon.Data;

public static class MockEntityHelper
{
    public static UserEntity GetUserEntity(Guid externalId, string email, string password)
    {
        return new UserEntity
        {
            ExternalId = externalId,
            Email = email,
            Name = Guid.NewGuid().ToString(),
            Surname = Guid.NewGuid().ToString(),
            Password = password,
            CreatedOn = DateTimeOffset.UtcNow,
            LastUpdatedOn = DateTimeOffset.UtcNow
        };
    }

    public static UserEntity GetUserEntity(Guid? externalId = null)
    {
        return new UserEntity
        {
            ExternalId = externalId ?? Guid.NewGuid(),
            Email = Guid.NewGuid().ToString(),
            Name = Guid.NewGuid().ToString(),
            Surname = Guid.NewGuid().ToString(),
            Password = Guid.NewGuid().ToString()
        };
    }
}

