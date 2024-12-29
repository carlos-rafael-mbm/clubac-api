using ClubApi.Application.Queries.AccessLogs.Dtos;
using ClubApi.Domain.Abstractions;
using MediatR;

namespace ClubApi.Application.Queries.AccessLogs.GetAccessLogByFilters;

public class GetAccessLogsByFiltersQueryHandler : IRequestHandler<GetAccessLogByFiltersQuery, IReadOnlyList<AccessLogDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAccessLogsByFiltersQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<AccessLogDto>> Handle(GetAccessLogByFiltersQuery request, CancellationToken cancellationToken)
    {
        return await GetByFilters(request.ClientId, request.StartDate, request.EndDate, cancellationToken);
    }

    private async Task<IReadOnlyList<AccessLogDto>> GetByFilters(int? clientId, DateTime? startDate, DateTime? endDate, CancellationToken cancellationToken)
    {
        var logs = await _unitOfWork.AccessLogs.GetFilteredAccessLogsAsync(clientId, startDate, endDate, cancellationToken);

        return logs.Select(log => new AccessLogDto
        {
            Id = log.Id,
            ClientId = log.ClientId,
            EntryTime = log.EntryTime,
            ExitTime = log.ExitTime
        }).ToList();
    }
}
