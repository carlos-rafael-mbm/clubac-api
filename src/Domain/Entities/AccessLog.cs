using ClubApi.Domain.Abstractions;

namespace ClubApi.Domain.Entities;

public sealed class AccessLog : Entity<long>
{
    private AccessLog() { }

    private AccessLog(
        long id,
        int clientId,
        DateTime entryTime,
        DateTime? exitTime
    ) : base(id)
    {
        ClientId = clientId;
        EntryTime = entryTime;
        ExitTime = exitTime;
    }

    public int ClientId { get; private set; }
    public DateTime EntryTime { get; private set; }
    public DateTime? ExitTime { get; private set; }

    public static AccessLog RegisterEntry(
        int clientId,
        DateTime entryTime
    )
    {
        return new AccessLog(0, clientId, entryTime, null);
    }

    public static AccessLog RegisterTest(
        long id,
        int clientId,
        DateTime entryTime,
        DateTime? exitTime
    )
    {
        return new AccessLog(id, clientId, entryTime, exitTime);
    }

    public void RegisterExit(DateTime exitTime)
    {
        ExitTime = exitTime;
    }
}
