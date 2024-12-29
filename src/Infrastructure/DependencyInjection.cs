using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ClubApi.Infrastructure.Persistence;
using ClubApi.Domain.Abstractions;
using ClubApi.Infrastructure.UnitWork;
using ClubApi.Infrastructure.Repositories.Base;
using ClubApi.Infrastructure.Repositories;

namespace ClubApi.Infrastructure;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") ?? configuration.GetConnectionString("DefaultConnection");
        // _ = services.AddHttpContextAccessor();
        // _ = services.AddScoped(provider =>
        //     new EntityInterceptor("default_user"));
        services.AddScoped<EntityInterceptor>(provider =>
        {
            var userContextService = provider.GetRequiredService<IUserContextService>();
            return new EntityInterceptor(userContextService);
        });

        _ = services.AddDbContext<ApplicationDbContext>((provider, options) =>
        {
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        });

        _ = services.AddScoped(typeof(IRepository<,>), typeof(RepositoryBase<,>));
        _ = services.AddScoped<IUnitOfWork, UnitOfWork>();
        _ = services.AddScoped<IUserRepository, UserRepository>();
        _ = services.AddScoped<IClientRepository, ClientRepository>();
        _ = services.AddScoped<IAccessLogRepository, AccessLogRepository>();
        return services;
    }
}