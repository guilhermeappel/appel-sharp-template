namespace Appel.SharpTemplate.Domain.Models.User;

public sealed record UserAuthenticationModel
{
    public Guid ExternalId { get; set; }
    public string? Email { get; set; }
    public string? Token { get; set; }
}
