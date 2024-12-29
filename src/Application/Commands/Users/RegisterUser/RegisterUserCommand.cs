using MediatR;
using ClubApi.Application.Commands.Users.Dtos;

namespace ClubApi.Application.Commands.Users.RegisterUser;

public class RegisterUserCommand : IRequest<UserDto>
{
    public string Username { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public int RoleId { get; set; }
}
