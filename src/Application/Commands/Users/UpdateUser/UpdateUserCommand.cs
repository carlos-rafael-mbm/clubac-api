using MediatR;
using ClubApi.Application.Commands.Users.Dtos;

namespace ClubApi.Application.Commands.Users.UpdateUser;

public class UpdateUserCommand : IRequest<UserDto>
{
    public int Id { get; set; }
    public string? Username { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public int? RoleId { get; set; }
}
