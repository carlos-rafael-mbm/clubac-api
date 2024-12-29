using ClubApi.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClubApi.Infrastructure.Configurations;

public abstract class EntityMapBase<T, TEntityId> : IEntityTypeConfiguration<T> where T : Entity<TEntityId>
{
    void IEntityTypeConfiguration<T>.Configure(EntityTypeBuilder<T> entity)
    {
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).HasColumnName("id");
        entity.Property(e => e.UserCreated).HasColumnName("user_created").HasMaxLength(100).IsRequired();
        entity.Property(e => e.UserUpdated).HasColumnName("user_updated").HasMaxLength(100).IsRequired(false);
        entity.Property(e => e.CreatedAt).HasColumnName("created_at").IsRequired();
        entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").IsRequired(false);
        entity.Property(e => e.IsActive).HasColumnName("is_active").HasDefaultValue(true);
        Configure(entity);
    }

    protected abstract void Configure(EntityTypeBuilder<T> entity);
}