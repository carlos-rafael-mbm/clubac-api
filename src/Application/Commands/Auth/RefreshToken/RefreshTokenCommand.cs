using ClubApi.Application.Commands.Auth.Dtos;
using MediatR;

namespace ClubApi.Application.Commands.Auth.RefreshToken;

public class RefreshTokenCommand : IRequest<LoginDto>
{
    public string RefreshToken { get; set; }
}
