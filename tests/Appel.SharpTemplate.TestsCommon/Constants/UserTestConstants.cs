namespace Appel.SharpTemplate.TestsCommon.Constants;

public static class UserTestConstants
{
    public static class Entity
    {
        public static class NewUser
        {
            public const string EMAIL = "newuser@email.com";
            public const string PASSWORD = "NewuserPassword123!";
        }

        public static class User1
        {
#pragma warning disable CA2211 // Non-constant fields should not be visible
            public static Guid EXTERNAL_ID = Guid.NewGuid();
#pragma warning restore CA2211 // Non-constant fields should not be visible

            public const string EMAIL = "user1@email.com";
            public const string NAME = "User";
            public const string PASSWORD = "User1Password123!";
        }

        public static class User2
        {
#pragma warning disable CA2211 // Non-constant fields should not be visible
            public static Guid EXTERNAL_ID = Guid.NewGuid();
#pragma warning restore CA2211 // Non-constant fields should not be visible

            public const string EMAIL = "user2@email.com";
            public const string PASSWORD = "User2Password123!";
        }
    }
}
