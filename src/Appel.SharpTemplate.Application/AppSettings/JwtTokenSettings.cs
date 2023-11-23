namespace Appel.SharpTemplate.Application.AppSettings;

public sealed record JwtTokenSettings
{
    public int ExpiryMinutes { get; set; }
    public string? SecretKey { get; set; }
}
