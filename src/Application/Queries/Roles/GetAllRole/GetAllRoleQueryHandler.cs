using ClubApi.Application.Queries.Roles.Dtos;
using ClubApi.Application.Queries.Users.GetAllRole;
using ClubApi.Domain.Abstractions;
using ClubApi.Domain.Entities;
using MediatR;

namespace ClubApi.Application.Queries.Roles.GetAllRole;

public class GetAllRoleQueryHandler : IRequestHandler<GetAllRoleQuery, IEnumerable<RoleDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllRoleQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<RoleDto>> Handle(GetAllRoleQuery request, CancellationToken cancellationToken)
    {
        return await Get(cancellationToken);
    }

    private async Task<IEnumerable<RoleDto>> Get(CancellationToken cancellationToken)
    {
        var roles = await _unitOfWork.Repository<Role, int>().GetAllAsync(cancellationToken);

        return roles.Select(Role => new RoleDto
        {
            Id = Role.Id,
            Name = Role.Name,
            IsActive = Role.IsActive,
        });
    }
}
