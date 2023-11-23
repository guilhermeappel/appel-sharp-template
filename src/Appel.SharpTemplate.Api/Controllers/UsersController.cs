using Appel.SharpTemplate.Api.Contracts.User;
using Appel.SharpTemplate.Api.Mappers;
using Appel.SharpTemplate.Api.Validators;
using Appel.SharpTemplate.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Appel.SharpTemplate.Api.Controllers;

public class UsersController : BaseController
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    [ActionName(nameof(LoginAsync))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> LoginAsync(UserLoginContract userLoginContract, CancellationToken cancellationToken)
    {
        var validator = new UserLoginValidator();
        var validationResult = await validator.ValidateAsync(userLoginContract, cancellationToken);
        if (!validationResult.IsValid)
        {
            return ValidationProblem(validationResult);
        }

        var mapper = new UserMapper();
        var userLoginModel = mapper.Map(userLoginContract);

        var result = await _userService.LoginAsync(userLoginModel, cancellationToken);
        return result.Match(Ok, Problem);
    }

    [AllowAnonymous]
    [HttpPost("register")]
    [ActionName(nameof(RegisterAsync))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RegisterAsync(UserRegisterContract userRegisterContract, CancellationToken cancellationToken)
    {
        var validator = new UserRegisterValidator();
        var validationResult = await validator.ValidateAsync(userRegisterContract, cancellationToken);
        if (!validationResult.IsValid)
        {
            return ValidationProblem(validationResult);
        }

        var mapper = new UserMapper();
        var userRegisterModel = mapper.Map(userRegisterContract);

        var result = await _userService.RegisterAsync(userRegisterModel, cancellationToken);

        return result.Match(
            success => CreatedAtAction(nameof(GetByExternalIdAsync), new { externalId = result.Value.ExternalId }, success),
            errors => Problem(errors));
    }

    [HttpGet("{externalId}")]
    [ActionName(nameof(GetByExternalIdAsync))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByExternalIdAsync(string externalId, CancellationToken cancellationToken)
    {
        var result = await _userService.GetByExternalIdAsync(externalId, cancellationToken);
        return result.Match(Ok, Problem);
    }
}
