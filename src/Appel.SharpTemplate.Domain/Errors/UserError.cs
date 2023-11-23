using ErrorOr;

namespace Appel.SharpTemplate.Domain.Errors;

public static class UserError
{
    public static Error EmailDuplicated =>
        Error.Conflict(
            code: "UserError.EmailDuplicated",
            description: "Email is already in use.");

    public static Error InvalidCredentials =>
        Error.Conflict(
            code: "UserError.InvalidCredentials",
            description: "Invalid credentials.");

    public static Error UserNotFound =>
        Error.NotFound(
            code: "UserError.UserNotFound",
            description: "User not found.");
}
