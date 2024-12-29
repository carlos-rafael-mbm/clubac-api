using ClubApi.Application.Queries.Roles.Dtos;
using ClubApi.Application.Constants.Validations;
using ClubApi.Application.Queries.Users.Dtos;
using ClubApi.Domain.Abstractions;
using MediatR;

namespace ClubApi.Application.Queries.Users.GetUserById;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetUserByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        return await GetById(request.Id, cancellationToken);
    }

    private async Task<UserDto> GetById(int id, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetUserByIdWithRoleAsync(id, cancellationToken) ?? throw new KeyNotFoundException(string.Format(ValidationMessages.NOT_FOUND, "Usuario", id));

        return new UserDto
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
            },
        };
    }
}
