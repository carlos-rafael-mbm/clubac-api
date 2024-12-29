using ClubApi.Application.Commands.Roles.Dtos;
using ClubApi.Application.Commands.Users.Dtos;
using ClubApi.Application.Constants.Validations;
using ClubApi.Domain.Abstractions;
using ClubApi.Domain.Entities;
using MediatR;

namespace ClubApi.Application.Commands.Users.RegisterUser;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, UserDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public RegisterUserCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<UserDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var role = await _unitOfWork.Repository<Role, int>().GetByIdAsync(request.RoleId, cancellationToken) ?? throw new KeyNotFoundException(string.Format(ValidationMessages.NOT_FOUND, "Rol", request.RoleId));

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var user = User.Create(
                request.Username,
                request.Name,
                request.Email,
                request.Password,
                request.RoleId
            );

            var userCreated = await _unitOfWork.Repository<User, int>().AddAsync(user, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            var userDto = new UserDto
            {
                Id = userCreated.Id,
                Username = userCreated.Username,
                Name = userCreated.Name,
                Email = userCreated.Email,
                IsActive = userCreated.IsActive,
                Role = new RoleDto
                {
                    Id = role.Id,
                    Name = role.Name,
                    IsActive = role.IsActive
                }
            };

            return userDto;
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw new InvalidOperationException(ValidationMessages.INTERNAL_SERVER_ERROR, ex);
        }
    }
}
