namespace Appel.SharpTemplate.Domain.Models.User;

public sealed record UserLoginModel
{
    public string? Email { get; set; }
    public string? Password { get; set; }
}
