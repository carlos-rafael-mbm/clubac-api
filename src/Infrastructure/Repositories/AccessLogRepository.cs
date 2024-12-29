using ClubApi.Domain.Abstractions;
using ClubApi.Domain.Entities;
using ClubApi.Infrastructure.Persistence;
using ClubApi.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace ClubApi.Infrastructure.Repositories;

public class AccessLogRepository : RepositoryBase<AccessLog, long>, IAccessLogRepository
{
    public AccessLogRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IReadOnlyList<AccessLog>> GetFilteredAccessLogsAsync(int? clientId, DateTime? startDate, DateTime? endDate, CancellationToken cancellationToken = default)
    {
        var query = _context.AccessLogs.AsQueryable();

        if (clientId.HasValue)
        {
            query = query.Where(log => log.ClientId == clientId.Value);
        }

        if (startDate.HasValue)
        {
            query = query.Where(log => log.EntryTime >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(log => log.EntryTime <= endDate.Value || (log.ExitTime.HasValue && log.ExitTime <= endDate.Value));
        }

        return await query.ToListAsync(cancellationToken);
    }
}