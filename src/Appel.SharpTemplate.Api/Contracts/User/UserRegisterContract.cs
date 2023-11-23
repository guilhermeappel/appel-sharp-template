namespace Appel.SharpTemplate.Api.Contracts.User;

public sealed record UserRegisterContract
{
    public string? Email { get; set; }
    public string? Name { get; set; }
    public string? Password { get; set; }
    public string? Surname { get; set; }
}
