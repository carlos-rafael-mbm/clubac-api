using ClubApi.Application.Queries.Users.Dtos;
using MediatR;

namespace ClubApi.Application.Queries.Users.GetUserById;

public sealed record GetUserByIdQuery(int Id) : IRequest<UserDto>;
