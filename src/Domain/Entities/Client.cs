using ClubApi.Domain.Abstractions;

namespace ClubApi.Domain.Entities;

public sealed class Client : Entity<int>
{
    private Client() { }

    private Client(
        int id,
        string name,
        int clientTypeId,
        string? email,
        string? phone
    ) : base(id)
    {
        Name = name;
        ClientTypeId = clientTypeId;
        Email = email;
        Phone = phone;
    }

    public string Name { get; private set; }
    public int ClientTypeId { get; private set; }
    public ClientType ClientType { get; private set; }
    public string? Email { get; private set; }
    public string? Phone { get; private set; }

    public static Client Create(
        string name,
        int clientTypeId,
        string? email,
        string? phone
    )
    {
        return new Client(0, name, clientTypeId, email, phone);
    }

    public static Client CreateTest(
        int id,
        string name,
        int clientTypeId,
        string? email,
        string? phone,
        ClientType clientType
    )
    {
        return new Client(id, name, clientTypeId, email, phone)
        {
            ClientType = clientType
        };
    }

    public void Update(
        string? name,
        int? clientTypeId,
        string? email,
        string? phone
    )
    {
        Name = name ?? Name;
        Email = email ?? Email;
        Phone = phone ?? Phone;
        if (clientTypeId.HasValue)
        {
            if (clientTypeId <= 0)
            {
                throw new ArgumentException("RoleId must be a valid role identifier.", nameof(clientTypeId));
            }

            ClientTypeId = clientTypeId.Value;
        }
    }
}
