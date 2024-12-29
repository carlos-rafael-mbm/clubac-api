using ClubApi.Application.Queries.Roles.Dtos;
using ClubApi.Application.Queries.Users.Dtos;
using ClubApi.Domain.Abstractions;
using MediatR;

namespace ClubApi.Application.Queries.Users.GetAllUser;

public class GetAllUserQueryHandler : IRequestHandler<GetAllUserQuery, IEnumerable<UserDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllUserQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<UserDto>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
    {
        return await Get(cancellationToken);
    }

    private async Task<IEnumerable<UserDto>> Get(CancellationToken cancellationToken)
    {
        var users = await _unitOfWork.Users.GetAllUsersWithRolesAsync(cancellationToken);

        return users.Select(user => new UserDto
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
        });
    }
}
