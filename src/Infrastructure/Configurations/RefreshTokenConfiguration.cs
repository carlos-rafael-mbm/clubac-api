using ClubApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClubApi.Infrastructure.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> entity)
    {
        entity.ToTable("refresh_tokens");
        entity.HasKey(rt => rt.Id);
        entity.Property(rt => rt.Id).HasColumnName("id");
        entity.Property(rt => rt.Token).HasColumnName("token").IsRequired().HasMaxLength(256);
        entity.Property(rt => rt.Expiration).HasColumnName("expiration").IsRequired();
        entity.Property(rt => rt.IsRevoked).HasColumnName("is_revoked").HasDefaultValue(false);
        entity.Property(rt => rt.IsUsed).HasColumnName("is_used").HasDefaultValue(false);
        entity.Property(rt => rt.UserId).HasColumnName("user_id").IsRequired();
        entity.HasOne(rt => rt.User)
            .WithMany()
            .HasForeignKey(rt => rt.UserId);
    }
}
