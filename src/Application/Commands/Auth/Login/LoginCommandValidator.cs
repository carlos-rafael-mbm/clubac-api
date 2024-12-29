using ClubApi.Application.Constants.Validations;
using FluentValidation;

namespace ClubApi.Application.Commands.Auth.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.EmailUsername)
            .NotEmpty().WithMessage(string.Format(ValidationMessages.IS_REQUIRED, "Usuario"))
            .MaximumLength(50).WithMessage(string.Format(ValidationMessages.MAXIMUM_LENGTH, "Usuario", 50));

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(string.Format(ValidationMessages.IS_REQUIRED, "Contraseña"))
            .MinimumLength(6).WithMessage(string.Format(ValidationMessages.MINIMUM_LENGTH, "Contraseña", 6));
    }
}
