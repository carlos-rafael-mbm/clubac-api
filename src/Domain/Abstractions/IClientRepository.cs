using ClubApi.Domain.Entities;

namespace ClubApi.Domain.Abstractions;

public interface IClientRepository : IRepository<Client, int>
{
    Task<IReadOnlyList<Client>> GetAllClientsWithClientTypesAsync(CancellationToken cancellationToken = default);
}
