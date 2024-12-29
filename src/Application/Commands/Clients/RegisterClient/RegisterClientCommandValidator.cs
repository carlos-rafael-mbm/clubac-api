using ClubApi.Application.Constants.Validations;
using FluentValidation;

namespace ClubApi.Application.Commands.Clients.RegisterClient;

public class RegisterClientCommandValidator : AbstractValidator<RegisterClientCommand>
{
    public RegisterClientCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(string.Format(ValidationMessages.IS_REQUIRED, "Nombre"))
            .MaximumLength(50).WithMessage(string.Format(ValidationMessages.MAXIMUM_LENGTH, "Nombre", 50));

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(string.Format(ValidationMessages.IS_REQUIRED, "Email"))
            .EmailAddress().WithMessage(string.Format(ValidationMessages.EMAIL_FORMAT, "Email"))
            .MaximumLength(50).WithMessage(string.Format(ValidationMessages.MAXIMUM_LENGTH, "Email", 50));

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage(string.Format(ValidationMessages.IS_REQUIRED, "Teléfono"))
            .MaximumLength(20).WithMessage(string.Format(ValidationMessages.MAXIMUM_LENGTH, "Teléfono", 20));

        RuleFor(x => x.ClientTypeId)
            .GreaterThan(0).WithMessage(string.Format(ValidationMessages.POSITIVE_INTEGER, "Id de Tipo de Cliente"));
    }
}
