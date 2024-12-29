using ClubApi.Domain.Abstractions;
using ClubApi.Domain.Entities;
using ClubApi.Infrastructure.Persistence;
using ClubApi.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace ClubApi.Infrastructure.Repositories;

public class ClientRepository : RepositoryBase<Client, int>, IClientRepository
{
    public ClientRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IReadOnlyList<Client>> GetAllClientsWithClientTypesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Clients.Include(c => c.ClientType).ToListAsync(cancellationToken);
    }
}