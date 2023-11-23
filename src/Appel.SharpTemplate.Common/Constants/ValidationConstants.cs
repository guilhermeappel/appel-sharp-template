namespace Appel.SharpTemplate.Common.Constants;

public static class ValidationConstants
{
    public static class User
    {
        public static class Database
        {
            public const byte PASSWORD_MAX_LENGTH = 120;
        }

        public static class Input
        {
            public const byte PASSWORD_MAX_LENGTH = 20;
        }

        public static class Shared
        {
            public const byte EMAIL_MAX_LENGTH = 100;
            public const byte NAME_MAX_LENGTH = 100;
            public const byte PASSWORD_MIN_LENGTH = 6;
            public const byte SURNAME_MAX_LENGTH = 100;
        }
    }
}
