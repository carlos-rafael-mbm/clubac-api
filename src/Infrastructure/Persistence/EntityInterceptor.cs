using ClubApi.Domain.Abstractions;
using ClubApi.Infrastructure.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ClubApi.Infrastructure.Persistence;

public class EntityInterceptor : SaveChangesInterceptor
{
    private readonly IUserContextService _contextService;

    public EntityInterceptor(IUserContextService contextService)
    {
        _contextService = contextService;
    }

    private string GetUsername()
    {
        return _contextService.GetUsername();
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        ApplyAuditableProperties(eventData.Context!);
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void ApplyAuditableProperties(DbContext context)
    {
        if (context == null) return;

        foreach (var entry in context.ChangeTracker.Entries())
        {
            var entityType = entry.Entity.GetType();
            var baseType = entityType.BaseType;

            if (baseType != null && baseType.IsGenericType && baseType.GetGenericTypeDefinition() == typeof(Entity<>))
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        SetPropertyValue(entry.Entity, "CreatedAt", Util.GetDate());
                        SetPropertyValue(entry.Entity, "UserCreated", GetUsername());
                        SetPropertyValue(entry.Entity, "IsActive", true);
                        break;
                    case EntityState.Modified:
                        SetPropertyValue(entry.Entity, "UpdatedAt", Util.GetDate());
                        SetPropertyValue(entry.Entity, "UserUpdated", GetUsername());
                        break;
                    default:
                        break;
                }
            }
        }
    }

    private static void SetPropertyValue(object entity, string propertyName, object value)
    {
        var property = entity.GetType().GetProperty(propertyName);
        if (property != null && property.CanWrite)
        {
            property.SetValue(entity, value);
        }
    }
}