using MediatR;
using ClubApi.Application.Commands.Clients.Dtos;

namespace ClubApi.Application.Commands.Clients.RegisterClient;

public class RegisterClientCommand : IRequest<ClientDto>
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public int ClientTypeId { get; set; }
}
