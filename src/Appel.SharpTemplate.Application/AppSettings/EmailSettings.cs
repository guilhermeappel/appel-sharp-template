namespace Appel.SharpTemplate.Application.AppSettings;

public sealed record EmailSettings
{
    public string? Password { get; set; }
    public string? SmtpHost { get; set; }
    public string? SmtpPort { get; set; }
    public string? TokenSecretKey { get; set; }
    public string? User { get; set; }
}
