using ErrorOr;
using Microsoft.AspNetCore.Http;

namespace Appel.SharpTemplate.Common.ExtensionMethods;

public static class ErrorTypeExtensions
{
    public static int ToHttpStatusCode(this ErrorType errorType)
    {
        return errorType switch
        {
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };
    }
}
