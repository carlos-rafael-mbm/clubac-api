namespace ClubApi.Application.Commands.AccessLogs.Dtos;

public class AccessLogDto
{
    public long Id { get; set; }
    public int ClientId { get; set; }
    public DateTime EntryTime { get; set; }
    public DateTime? ExitTime { get; set; }
}
