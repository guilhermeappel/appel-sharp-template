namespace Appel.SharpTemplate.Application.AppSettings;

public sealed record Argon2HashSettings
{
    public string? Salt { get; set; }
    public string? SecretKey { get; set; }
}
