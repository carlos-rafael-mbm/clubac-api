using System.Linq.Expressions;
using ClubApi.Domain.Abstractions;
using ClubApi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ClubApi.Infrastructure.Repositories.Base;

public class RepositoryBase<T, TKey> : IRepository<T, TKey> where T : class
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<T> dbSet;

    public RepositoryBase(ApplicationDbContext context)
    {
        _context = context;
        dbSet = context.Set<T>();
    }

    public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await dbSet.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<T?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
    {
        return await dbSet.FindAsync(id, cancellationToken);
    }

    public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        return (await dbSet.AddAsync(entity, cancellationToken)).Entity;
    }

    public void Update(T entity)
    {
        dbSet.Update(entity);
    }

    public void Delete(T entity)
    {
        dbSet.Remove(entity);
    }

    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await dbSet.AnyAsync(predicate, cancellationToken);
    }
}