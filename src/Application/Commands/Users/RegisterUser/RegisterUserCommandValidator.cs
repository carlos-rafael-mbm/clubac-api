using ClubApi.Application.Constants.Validations;
using FluentValidation;

namespace ClubApi.Application.Commands.Users.RegisterUser;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage(string.Format(ValidationMessages.IS_REQUIRED, "Usuario"))
            .MaximumLength(50).WithMessage(string.Format(ValidationMessages.MAXIMUM_LENGTH, "Usuario", 50));

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(string.Format(ValidationMessages.IS_REQUIRED, "Nombre"))
            .MaximumLength(50).WithMessage(string.Format(ValidationMessages.MAXIMUM_LENGTH, "Nombre", 50));

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(string.Format(ValidationMessages.IS_REQUIRED, "Email"))
            .EmailAddress().WithMessage(string.Format(ValidationMessages.EMAIL_FORMAT, "Email"))
            .MaximumLength(50).WithMessage(string.Format(ValidationMessages.MAXIMUM_LENGTH, "Email", 50));

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(string.Format(ValidationMessages.IS_REQUIRED, "Contraseña"))
            .MinimumLength(6).WithMessage(string.Format(ValidationMessages.MINIMUM_LENGTH, "Contraseña", 6));

        RuleFor(x => x.RoleId)
            .GreaterThan(0).WithMessage(string.Format(ValidationMessages.POSITIVE_INTEGER, "Id de Rol"));
    }
}
