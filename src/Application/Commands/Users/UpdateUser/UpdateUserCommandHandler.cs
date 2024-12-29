using ClubApi.Application.Commands.Roles.Dtos;
using ClubApi.Application.Commands.Users.Dtos;
using ClubApi.Application.Constants.Validations;
using ClubApi.Domain.Abstractions;
using ClubApi.Domain.Entities;
using MediatR;

namespace ClubApi.Application.Commands.Users.UpdateUser;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUserCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetUserByIdWithRoleAsync(request.Id, cancellationToken) ?? throw new KeyNotFoundException(string.Format(ValidationMessages.NOT_FOUND, "Usuario", request.Id));

        if (request.RoleId.HasValue)
        {
            _ = await _unitOfWork.Repository<Role, int>().GetByIdAsync(request.RoleId.Value, cancellationToken) ?? throw new KeyNotFoundException(string.Format(ValidationMessages.NOT_FOUND, "Rol", request.RoleId));
        }

        user.Update(request.Username, request.Name, request.Email, request.Password, request.RoleId);

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            _unitOfWork.Users.Update(user);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            var userDto = new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Name = user.Name,
                Email = user.Email,
                IsActive = user.IsActive,
                Role = new RoleDto
                {
                    Id = user.Role.Id,
                    Name = user.Role.Name,
                    IsActive = user.Role.IsActive
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
