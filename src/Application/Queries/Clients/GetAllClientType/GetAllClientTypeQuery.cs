using ClubApi.Application.Queries.Clients.Dtos;
using MediatR;

namespace ClubApi.Application.Queries.Clients.GetAllClientType;

public sealed record GetAllClientTypeQuery() : IRequest<IEnumerable<ClientTypeDto>>;
