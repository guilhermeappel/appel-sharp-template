using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Appel.SharpTemplate.Api.Controllers;

[AllowAnonymous]
[Route("error")]
[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorsController : BaseController
{
    public IActionResult GlobalErrorHandler()
    {
        return Problem();
    }
}
