namespace ClubApi.Domain.Abstractions;

public abstract class Entity<TEntityId>
{
    protected Entity() { }

    protected Entity(TEntityId id)
    {
        Id = id;
    }

    public TEntityId? Id { get; init; }
    public string UserCreated { get; set; }
    public string? UserUpdated { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; }
}
