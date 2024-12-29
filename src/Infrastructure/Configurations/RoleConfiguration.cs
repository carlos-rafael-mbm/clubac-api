using ClubApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClubApi.Infrastructure.Configurations;

public class RoleConfiguration : EntityMapBase<Role, int>
{
    protected override void Configure(EntityTypeBuilder<Role> entity)
    {
        entity.ToTable("roles");
        entity.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(100);
    }
}
