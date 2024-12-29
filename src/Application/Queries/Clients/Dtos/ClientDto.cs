namespace ClubApi.Application.Queries.Clients.Dtos;

public class ClientDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public ClientTypeDto ClientType { get; set; }
    public bool IsActive { get; set; }
}