using ClubApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClubApi.Infrastructure.Configurations;

public class UserConfiguration : EntityMapBase<User, int>
{
    protected override void Configure(EntityTypeBuilder<User> entity)
    {
        entity.ToTable("users");
        entity.Property(e => e.Username).HasColumnName("username").IsRequired().HasMaxLength(50);
        entity.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(50);
        entity.Property(e => e.Email).HasColumnName("email").IsRequired().HasMaxLength(50);
        entity.Property(e => e.Password).HasColumnName("password").IsRequired();
        entity.Property(e => e.RoleId).HasColumnName("role_id").IsRequired();
        entity.HasOne(e => e.Role)
            .WithMany()
            .HasForeignKey(e => e.RoleId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
