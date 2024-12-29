using System.Collections;
using ClubApi.Domain.Abstractions;
using ClubApi.Infrastructure.Persistence;
using ClubApi.Infrastructure.Repositories;
using ClubApi.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore.Storage;

namespace ClubApi.Infrastructure.UnitWork;

public class UnitOfWork : IUnitOfWork
{
    private Hashtable? _repositories;
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction transaction;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken)
    {
        transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (transaction != null)
        {
            await transaction.CommitAsync(cancellationToken);
            await transaction.DisposeAsync();
            transaction = null!;
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (transaction != null)
        {
            await transaction.RollbackAsync(cancellationToken);
            await transaction.DisposeAsync();
            transaction = null!;
        }
    }

    public async Task DisposeTransactionAsync()
    {
        if (transaction != null)
        {
            await transaction.DisposeAsync();
            transaction = null!;
        }
    }

    public void Dispose()
    {
        transaction?.Dispose();
        _context.Dispose();
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public IRepository<TEntity, TKey> Repository<TEntity, TKey>() where TEntity : class
    {
        _repositories ??= new Hashtable();

        var type = typeof(TEntity).Name;

        if (!_repositories.ContainsKey(type))
        {
            var repositoryType = typeof(RepositoryBase<,>);
            var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity), typeof(TKey)), _context);
            _repositories.Add(type, repositoryInstance);
        }

        return (IRepository<TEntity, TKey>)_repositories[type]!;
    }

    private UserRepository? _userRepository;
    public IUserRepository Users => _userRepository ??= new UserRepository(_context);

    private ClientRepository? _clientRepository;
    public IClientRepository Clients => _clientRepository ??= new ClientRepository(_context);

    private AccessLogRepository? _accessLogRepository;
    public IAccessLogRepository AccessLogs => _accessLogRepository ??= new AccessLogRepository(_context);
}