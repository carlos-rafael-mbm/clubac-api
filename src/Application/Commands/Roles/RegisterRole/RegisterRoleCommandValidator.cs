using ClubApi.Application.Commands.Roles.RegisterRole;
using FluentValidation;

namespace ClubApi.Application.Validators;

public class RegisterRoleCommandValidator : AbstractValidator<RegisterRoleCommand>
{
    public RegisterRoleCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");
    }
}
