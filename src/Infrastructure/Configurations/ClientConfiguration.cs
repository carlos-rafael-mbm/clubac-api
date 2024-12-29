using ClubApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClubApi.Infrastructure.Configurations;

public class ClientConfiguration : EntityMapBase<Client, int>
{
    protected override void Configure(EntityTypeBuilder<Client> entity)
    {
        entity.ToTable("clients");
        entity.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(100);
        entity.Property(e => e.ClientTypeId).HasColumnName("client_type_id").IsRequired();
        entity.Property(e => e.Email).HasColumnName("email").IsRequired(false).HasMaxLength(50);
        entity.Property(e => e.Phone).HasColumnName("phone").IsRequired(false).HasMaxLength(20);
        entity.HasOne(e => e.ClientType)
            .WithMany()
            .HasForeignKey(e => e.ClientTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
