using ClubApi.Application.Commands.AccessLogs.Dtos;
using MediatR;

namespace ClubApi.Application.Commands.AccessLogs.Entry;

public class RegisterEntryCommand : IRequest<AccessLogDto>
{
    public int ClientId { get; set; }
    public DateTime EntryTime { get; set; } = DateTime.UtcNow;
}
