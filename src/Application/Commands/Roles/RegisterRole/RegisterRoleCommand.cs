using MediatR;
using ClubApi.Domain.Abstractions;
using ClubApi.Application.Commands.Roles.Dtos;

namespace ClubApi.Application.Commands.Roles.RegisterRole;

public class RegisterRoleCommand : IRequest<RoleDto>
{
    public string Name { get; set; }
}
