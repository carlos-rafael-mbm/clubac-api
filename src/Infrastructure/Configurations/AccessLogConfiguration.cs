using ClubApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClubApi.Infrastructure.Configurations;

public class AccessLogConfiguration : EntityMapBase<AccessLog, long>
{
    protected override void Configure(EntityTypeBuilder<AccessLog> entity)
    {
        entity.ToTable("access_logs");
        entity.Property(e => e.ClientId).HasColumnName("client_id").IsRequired();
        entity.Property(e => e.EntryTime).HasColumnName("entry_time").IsRequired();
        entity.Property(e => e.ExitTime).HasColumnName("exit_time").IsRequired(false);
        entity.HasOne<Client>()
            .WithMany()
            .HasForeignKey(e => e.ClientId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
