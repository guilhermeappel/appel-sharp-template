using Appel.SharpTemplate.Api.Contracts.User;

namespace Appel.SharpTemplate.FunctionalTests.Data;
public class MockContractHelper
{
    public static UserLoginContract GetUserLoginContract(string email, string password)
    {
        return new UserLoginContract
        {
            Email = email,
            Password = password
        };
    }

    public static UserRegisterContract GetUserRegisterContract(string email, string password)
    {
        return new UserRegisterContract
        {
            Email = email,
            Name = Guid.NewGuid().ToString(),
            Password = password,
            Surname = Guid.NewGuid().ToString()
        };
    }
}
