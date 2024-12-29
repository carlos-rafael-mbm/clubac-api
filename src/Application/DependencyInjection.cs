using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using ClubApi.Application.Behaviours;
using Microsoft.Extensions.Configuration;
using ClubApi.Application.Configurations;

namespace ClubApi.Application;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtConfig = new JwtConfiguration();
        configuration.GetSection("Jwt").Bind(jwtConfig);

        jwtConfig.Secret = Environment.GetEnvironmentVariable("JWT_SECRET") ?? jwtConfig.Secret;
        var accessToken = Environment.GetEnvironmentVariable("JWT_AccessTokenExpirationMinutes");
        jwtConfig.AccessTokenExpirationMinutes = accessToken != null ? int.Parse(accessToken) : jwtConfig.AccessTokenExpirationMinutes;
        var refreshToken = Environment.GetEnvironmentVariable("JWT_RefreshTokenExpirationDays");
        jwtConfig.RefreshTokenExpirationDays = refreshToken != null ? int.Parse(refreshToken) : jwtConfig.RefreshTokenExpirationDays;

        _ = services.AddSingleton(jwtConfig);

        _ = services.AddMediatR(Assembly.GetExecutingAssembly());
        _ = services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly(), ServiceLifetime.Transient);
        _ = services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

        return services;
    }
}