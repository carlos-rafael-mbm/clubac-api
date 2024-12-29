namespace ClubApi.Application.Commands.Auth.Dtos;

public class LoginDto
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public string Username { get; set; }
    public string Role { get; set; }
    public DateTime AccessTokenExpiration { get; set; }
}
