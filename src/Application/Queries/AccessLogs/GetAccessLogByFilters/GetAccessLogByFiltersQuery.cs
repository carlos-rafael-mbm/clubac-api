using ClubApi.Application.Queries.AccessLogs.Dtos;
using MediatR;

namespace ClubApi.Application.Queries.AccessLogs.GetAccessLogByFilters;

public class GetAccessLogByFiltersQuery : IRequest<IReadOnlyList<AccessLogDto>>
{
    public int? ClientId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}