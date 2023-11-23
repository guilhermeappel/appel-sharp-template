using Appel.SharpTemplate.Application.AppSettings;

namespace Appel.SharpTemplate.UnitTests.Data;

public static class MockSettingsHelper
{
    public static EmailSettings GetEmailSettings()
    {
        return new EmailSettings
        {
            Password = Guid.NewGuid().ToString(),
            SmtpHost = Guid.NewGuid().ToString(),
            SmtpPort = Guid.NewGuid().ToString(),
            TokenSecretKey = Guid.NewGuid().ToString("N"),
            User = Guid.NewGuid().ToString()
        };
    }
}
