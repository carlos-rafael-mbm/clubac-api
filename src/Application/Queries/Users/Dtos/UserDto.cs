using ClubApi.Application.Queries.Roles.Dtos;

namespace ClubApi.Application.Queries.Users.Dtos;

public class UserDto
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public RoleDto Role { get; set; }
    public bool IsActive { get; set; }
}