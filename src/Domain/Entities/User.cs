using ClubApi.Domain.Abstractions;

namespace ClubApi.Domain.Entities;

public sealed class User : Entity<int>
{
    private User() { }

    private User(
        int id,
        string username,
        string name,
        string email,
        string password,
        int roleId
    ) : base(id)
    {
        Username = username;
        Name = name;
        Email = email;
        Password = password;
        RoleId = roleId;
    }

    public string Username { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string Password { get; private set; }
    public int RoleId { get; private set; }
    public Role Role { get; private set; }

    public static User Create(
        string username,
        string name,
        string email,
        string password,
        int roleId
    )
    {
        return new User(0, username, name, email, HashPassword(password), roleId);
    }

    public static User CreateTest(
        int id,
        string username,
        string name,
        string email,
        string password,
        int roleId,
        Role role
    )
    {
        return new User(id, username, name, email, HashPassword(password), roleId)
        {
            Role = role
        };
    }

    public void Update(
        string? username,
        string? name,
        string? email,
        string? password,
        int? roleId
    )
    {
        Username = username ?? Username;
        Name = name ?? Name;
        Email = email ?? Email;
        Password = password != null ? HashPassword(password) : Password;
        if (roleId.HasValue)
        {
            RoleId = roleId.Value;
        }
    }

    public void AssignRole(int roleId)
    {
        RoleId = roleId;
    }

    public void Delete()
    {
        IsActive = false;
    }

    private static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool VerifyPassword(string password)
    {
        return BCrypt.Net.BCrypt.Verify(password, Password);
    }
}