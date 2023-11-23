using Appel.SharpTemplate.Api.Contracts.User;
using Appel.SharpTemplate.Common.Constants;
using FluentValidation;

namespace Appel.SharpTemplate.Api.Validators;

public sealed class UserRegisterValidator : AbstractValidator<UserRegisterContract>
{
    public UserRegisterValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .MaximumLength(ValidationConstants.User.Input.PASSWORD_MAX_LENGTH)
            .MinimumLength(ValidationConstants.User.Shared.PASSWORD_MIN_LENGTH);

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(ValidationConstants.User.Shared.NAME_MAX_LENGTH);

        RuleFor(x => x.Surname)
            .NotEmpty()
            .MaximumLength(ValidationConstants.User.Shared.SURNAME_MAX_LENGTH);
    }
}
