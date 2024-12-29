using ClubApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClubApi.Infrastructure.Configurations;

public class MembershipConfiguration : EntityMapBase<Membership, int>
{
    protected override void Configure(EntityTypeBuilder<Membership> entity)
    {
        entity.ToTable("memberships");
        entity.Property(e => e.ClientId).HasColumnName("client_id").IsRequired();
        entity.Property(e => e.MonthlyVisits).HasColumnName("monthly_visits").IsRequired();
        entity.Property(e => e.MonthlyFee).HasColumnName("monthly_fee").IsRequired().HasColumnType("decimal(10,2)");
        entity.Property(e => e.StartDate).HasColumnName("start_date").IsRequired().HasColumnType("date");
        entity.Property(e => e.EndDate).HasColumnName("end_date").IsRequired().HasColumnType("date");
        entity.HasOne<Client>()
            .WithMany()
            .HasForeignKey(e => e.ClientId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
