using ClubApi.Application.Queries.Clients.Dtos;
using MediatR;

namespace ClubApi.Application.Queries.Clients.GetAllClient;

public sealed record GetAllClientQuery() : IRequest<IEnumerable<ClientDto>>;
