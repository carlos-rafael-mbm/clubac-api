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

namespace ClubApi.Application.Commands.Auth.RefreshToken;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, LoginDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly JwtConfiguration _jwtConfig;

    public RefreshTokenCommandHandler(IUnitOfWork unitOfWork, JwtConfiguration jwtConfig)
    {
        _unitOfWork = unitOfWork;
        _jwtConfig = jwtConfig;
    }

    public async Task<LoginDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var refreshToken = await _unitOfWork.Users.GetRefreshTokenAsync(request.RefreshToken, cancellationToken)
                         ?? throw new UnauthorizedAccessException(ValidationMessages.INVALID_REFRESH_TOKEN);

        if (refreshToken.IsUsed || refreshToken.IsRevoked || refreshToken.Expiration < DateTime.UtcNow)
        {
            throw new UnauthorizedAccessException(ValidationMessages.INVALID_EXPIRED_REFRESH_TOKEN);
        }

        refreshToken.IsUsed = true;

        var user = refreshToken.User;
        var accessToken = GenerateJwt(user);
        var newRefreshToken = GenerateRefreshToken();

        var newRefreshTokenEntity = new Domain.Entities.RefreshToken
        {
            Token = newRefreshToken,
            Expiration = DateTime.UtcNow.AddDays(_jwtConfig.RefreshTokenExpirationDays),
            UserId = user.Id
        };

        await _unitOfWork.Users.AddRefreshTokenAsync(newRefreshTokenEntity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new LoginDto
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken,
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
