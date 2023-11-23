namespace Appel.SharpTemplate.Api.Contracts.User;

public sealed record UserLoginContract
{
    public string? Email { get; set; }
    public string? Password { get; set; }
}
