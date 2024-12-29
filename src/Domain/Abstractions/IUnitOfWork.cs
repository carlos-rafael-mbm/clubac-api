namespace ClubApi.Domain.Abstractions;

public interface IUnitOfWork : IDisposable
{
    IRepository<TEntity, TKey> Repository<TEntity, TKey>() where TEntity : class;
    IUserRepository Users { get; }
    IClientRepository Clients { get; }
    IAccessLogRepository AccessLogs { get; }
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    Task DisposeTransactionAsync();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}