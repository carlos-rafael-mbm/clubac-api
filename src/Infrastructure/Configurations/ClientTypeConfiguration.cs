using ClubApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClubApi.Infrastructure.Configurations;

public class ClientTypeConfiguration : EntityMapBase<ClientType, int>
{
    protected override void Configure(EntityTypeBuilder<ClientType> entity)
    {
        entity.ToTable("client_types");
        entity.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(100);
    }
}
