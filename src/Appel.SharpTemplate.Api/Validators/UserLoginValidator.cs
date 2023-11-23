using Appel.SharpTemplate.Api.Contracts.User;
using FluentValidation;

namespace Appel.SharpTemplate.Api.Validators;

public sealed class UserLoginValidator : AbstractValidator<UserLoginContract>
{
    public UserLoginValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty();

        RuleFor(x => x.Password)
            .NotEmpty();
    }
}
