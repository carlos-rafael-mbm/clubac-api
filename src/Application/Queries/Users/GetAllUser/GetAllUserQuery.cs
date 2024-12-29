using ClubApi.Application.Queries.Users.Dtos;
using MediatR;

namespace ClubApi.Application.Queries.Users.GetAllUser;

public sealed record GetAllUserQuery() : IRequest<IEnumerable<UserDto>>;
