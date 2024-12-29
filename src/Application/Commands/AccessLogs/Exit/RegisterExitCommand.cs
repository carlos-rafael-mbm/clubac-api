using ClubApi.Application.Commands.AccessLogs.Dtos;
using MediatR;

namespace ClubApi.Application.Commands.AccessLogs.Exit;

public class RegisterExitCommand : IRequest<AccessLogDto>
{
    public long AccessLogId { get; set; }
    public DateTime ExitTime { get; set; } = DateTime.UtcNow;
}
