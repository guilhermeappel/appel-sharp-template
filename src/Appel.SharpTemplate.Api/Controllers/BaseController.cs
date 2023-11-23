using Appel.SharpTemplate.Common.ExtensionMethods;
using ErrorOr;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Appel.SharpTemplate.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public abstract class BaseController : ControllerBase
{
    protected IActionResult Problem(List<Error> errors)
    {
        if (!errors.Any())
        {
            return Problem();
        }

        var firstError = errors[0];
        var statusCode = firstError.Type.ToHttpStatusCode();

        return Problem(statusCode: statusCode, title: firstError.Description);
    }

    protected IActionResult ValidationProblem(ValidationResult validationResult)
    {
        var modelStateDictionary = new ModelStateDictionary();
        foreach (var error in validationResult.Errors)
        {
            modelStateDictionary.AddModelError(error.PropertyName, error.ErrorMessage);
        }

        return ValidationProblem(modelStateDictionary);
    }
}
