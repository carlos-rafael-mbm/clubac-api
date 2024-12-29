using ClubApi.Application.Constants.Validations;
using FluentValidation;

namespace ClubApi.Application.Commands.Users.UpdateUser;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(string.Format(ValidationMessages.IS_REQUIRED, "Id"))
            .GreaterThan(0).WithMessage(string.Format(ValidationMessages.POSITIVE_INTEGER, "Id"));

        RuleFor(x => x.Username)
            .MaximumLength(50).WithMessage(string.Format(ValidationMessages.MAXIMUM_LENGTH, "Usuario", 50))
            .When(x => !string.IsNullOrEmpty(x.Username));

        RuleFor(x => x.Name)
            .MaximumLength(50).WithMessage(string.Format(ValidationMessages.MAXIMUM_LENGTH, "Nombre", 50))
            .When(x => !string.IsNullOrEmpty(x.Name));

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage(string.Format(ValidationMessages.EMAIL_FORMAT, "Email"))
            .MaximumLength(50).WithMessage(string.Format(ValidationMessages.MAXIMUM_LENGTH, "Email", 50))
            .When(x => !string.IsNullOrEmpty(x.Email));

        RuleFor(x => x.Password)
            .MinimumLength(6).WithMessage(string.Format(ValidationMessages.MINIMUM_LENGTH, "Contraseña", 6))
            .When(x => !string.IsNullOrEmpty(x.Password));

        RuleFor(x => x.RoleId)
            .GreaterThan(0).WithMessage(string.Format(ValidationMessages.POSITIVE_INTEGER, "Id de Rol"))
            .When(x => x.RoleId.HasValue);
    }
}
