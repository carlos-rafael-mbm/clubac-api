using ClubApi.Domain.Entities;

namespace ClubApi.Domain.Abstractions;

public interface IAccessLogRepository : IRepository<AccessLog, long>
{
    Task<IReadOnlyList<AccessLog>> GetFilteredAccessLogsAsync(int? clientId, DateTime? startDate, DateTime? endDate, CancellationToken cancellationToken = default);
}
