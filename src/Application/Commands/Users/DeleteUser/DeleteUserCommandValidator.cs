using ClubApi.Application.Constants.Validations;
using FluentValidation;

namespace ClubApi.Application.Commands.Users.DeleteUser;

public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(string.Format(ValidationMessages.IS_REQUIRED, "Id"))
            .GreaterThan(0).WithMessage(string.Format(ValidationMessages.POSITIVE_INTEGER, "Id"));
    }
}
