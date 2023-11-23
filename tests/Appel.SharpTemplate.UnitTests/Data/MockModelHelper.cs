using Appel.SharpTemplate.Domain.Models.User;

namespace Appel.SharpTemplate.UnitTests.Data;

public class MockModelHelper
{
    public static UserLoginModel GetUserLoginModel()
    {
        return new UserLoginModel
        {
            Email = Guid.NewGuid().ToString(),
            Password = Guid.NewGuid().ToString()
        };
    }

    public static UserRegisterModel GetUserRegisterModel()
    {
        return new UserRegisterModel
        {
            Email = Guid.NewGuid().ToString(),
            Name = Guid.NewGuid().ToString(),
            Password = Guid.NewGuid().ToString(),
            Surname = Guid.NewGuid().ToString()
        };
    }
}
