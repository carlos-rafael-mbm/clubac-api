using ClubApi.Domain.Entities;

namespace ClubApi.Domain.Abstractions;

public interface IUserRepository : IRepository<User, int>
{
    Task<IReadOnlyList<User>> GetAllUsersWithRolesAsync(CancellationToken cancellationToken = default);
    Task<User?> GetUserByIdWithRoleAsync(int id, CancellationToken cancellationToken = default);
    Task<User?> GetByEmailOrUsernameAsync(string emailUsername, CancellationToken cancellationToken = default);
    Task<RefreshToken?> GetRefreshTokenAsync(string token, CancellationToken cancellationToken = default);
    Task AddRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);
}
