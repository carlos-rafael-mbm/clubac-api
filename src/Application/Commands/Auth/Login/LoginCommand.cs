using ClubApi.Application.Commands.Auth.Dtos;
using MediatR;

namespace ClubApi.Application.Commands.Auth.Login;

public class LoginCommand : IRequest<LoginDto>
{
    public string EmailUsername { get; set; }
    public string Password { get; set; }
}
