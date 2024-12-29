using ClubApi.Domain.Abstractions;
using ClubApi.Domain.Entities;
using ClubApi.Infrastructure.Persistence;
using ClubApi.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace ClubApi.Infrastructure.Repositories;

public class UserRepository : RepositoryBase<User, int>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IReadOnlyList<User>> GetAllUsersWithRolesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Users.Include(u => u.Role).ToListAsync(cancellationToken);
    }

    public async Task<User?> GetUserByIdWithRoleAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<User?> GetByEmailOrUsernameAsync(string emailUsername, CancellationToken cancellationToken = default)
    {
        return await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == emailUsername || u.Username == emailUsername, cancellationToken);
    }

    public async Task<RefreshToken?> GetRefreshTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        return await _context.RefreshTokens.Include(rt => rt.User).ThenInclude(u => u.Role).FirstOrDefaultAsync(rt => rt.Token == token, cancellationToken);
    }

    public async Task AddRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default)
    {
        await _context.RefreshTokens.AddAsync(refreshToken, cancellationToken);
    }
}