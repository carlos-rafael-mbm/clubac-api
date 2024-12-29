using ClubApi.Application.Queries.Roles.Dtos;
using MediatR;

namespace ClubApi.Application.Queries.Users.GetAllRole;

public sealed record GetAllRoleQuery() : IRequest<IEnumerable<RoleDto>>;
