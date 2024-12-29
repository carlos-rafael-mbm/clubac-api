using ClubApi.Application.Commands.Auth.Dtos;
using ClubApi.Application.Configurations;
using ClubApi.Application.Constants.Validations;
using ClubApi.Domain.Abstractions;
using ClubApi.Domain.Entities;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ClubApi.Application.Commands.Auth.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly JwtConfiguration _jwtConfig;

    public LoginCommandHandler(IUnitOfWork unitOfWork, JwtConfiguration jwtConfig)
    {
        _unitOfWork = unitOfWork;
        _jwtConfig = jwtConfig;
    }

    public async Task<LoginDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetByEmailOrUsernameAsync(request.EmailUsername, cancellationToken)
                   ?? throw new UnauthorizedAccessException(ValidationMessages.INVALID_CREDENTIALS);

        if (!user.VerifyPassword(request.Password))
        {
            throw new UnauthorizedAccessException(ValidationMessages.INVALID_CREDENTIALS);
        }

        var accessToken = GenerateJwt(user);
        var refreshToken = GenerateRefreshToken();

        var refreshTokenEntity = new Domain.Entities.RefreshToken
        {
            Token = refreshToken,
            Expiration = DateTime.UtcNow.AddDays(_jwtConfig.RefreshTokenExpirationDays),
            UserId = user.Id
        };

        await _unitOfWork.Users.AddRefreshTokenAsync(refreshTokenEntity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new LoginDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            Username = user.Username,
            Role = user.Role.Name,
            AccessTokenExpiration = DateTime.UtcNow.AddMinutes(_jwtConfig.AccessTokenExpirationMinutes)
        };
    }

    private string GenerateJwt(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role.Name)
        };

        var token = new JwtSecurityToken(
            issuer: _jwtConfig.Issuer,
            audience: _jwtConfig.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtConfig.AccessTokenExpirationMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string GenerateRefreshToken()
    {
        return Guid.NewGuid().ToString("N");
    }
}
