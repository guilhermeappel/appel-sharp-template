namespace Appel.SharpTemplate.Domain.Models.User;

public sealed record UserRegisterModel
{
    public string? Email { get; set; }
    public string? Name { get; set; }
    public string? Password { get; set; }
    public string? Surname { get; set; }
}
