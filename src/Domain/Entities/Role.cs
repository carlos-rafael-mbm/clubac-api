using ClubApi.Domain.Abstractions;

namespace ClubApi.Domain.Entities;

public sealed class Role : Entity<int>
{
    private Role() { }

    private Role(
        int id,
        string name
    ) : base(id)
    {
        Name = name;
    }

    public string Name { get; private set; }

    public static Role Create(string name)
    {
        return new Role(0, name);
    }

    public static Role CreateTest(int id, string name)
    {
        return new Role(id, name);
    }

    public void Update(string name)
    {
        Name = name;
    }
}