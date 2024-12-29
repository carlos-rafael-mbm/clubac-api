using ClubApi.Domain.Abstractions;

namespace ClubApi.Domain.Entities;

public sealed class Membership : Entity<int>
{
    private Membership() { }

    private Membership(
        int id,
        int clientId,
        int monthlyVisits,
        decimal monthlyFee,
        DateTime startDate,
        DateTime endDate
    ) : base(id)
    {
        ClientId = clientId;
        MonthlyVisits = monthlyVisits;
        MonthlyFee = monthlyFee;
        StartDate = startDate;
        EndDate = endDate;
    }

    public int ClientId { get; private set; }
    public int MonthlyVisits { get; private set; }
    public decimal MonthlyFee { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }

    public static Membership Create(
        int clientId,
        int monthlyVisits,
        decimal monthlyFee,
        DateTime startDate,
        DateTime endDate
    )
    {
        return new Membership(0, clientId, monthlyVisits, monthlyFee, startDate, endDate);
    }

    public void Update(
        int? monthlyVisits,
        decimal? monthlyFee,
        DateTime? startDate,
        DateTime? endDate
    )
    {
        MonthlyVisits = monthlyVisits ?? MonthlyVisits;
        MonthlyFee = monthlyFee ?? MonthlyFee;
        StartDate = startDate ?? StartDate;
        EndDate = endDate ?? EndDate;
    }
}
