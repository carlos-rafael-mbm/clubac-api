using ClubApi.Domain.Abstractions;

namespace ClubApi.Domain.Entities;

public sealed class ClientType : Entity<int>
{
    private ClientType() { }

    private ClientType(
        int id,
        string name
    ) : base(id)
    {
        Name = name;
    }

    public string Name { get; private set; }

    public static ClientType Create(string name)
    {
        return new ClientType(0, name);
    }

    public static ClientType CreateTest(int id, string name)
    {
        return new ClientType(id, name);
    }

    public void Update(string name)
    {
        Name = name;
    }
}