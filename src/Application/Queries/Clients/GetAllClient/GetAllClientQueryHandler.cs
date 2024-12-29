using ClubApi.Application.Queries.Clients.Dtos;
using ClubApi.Domain.Abstractions;
using MediatR;

namespace ClubApi.Application.Queries.Clients.GetAllClient;

public class GetAllClientQueryHandler : IRequestHandler<GetAllClientQuery, IEnumerable<ClientDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllClientQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<ClientDto>> Handle(GetAllClientQuery request, CancellationToken cancellationToken)
    {
        return await Get(cancellationToken);
    }

    private async Task<IEnumerable<ClientDto>> Get(CancellationToken cancellationToken)
    {
        var clients = await _unitOfWork.Clients.GetAllClientsWithClientTypesAsync(cancellationToken);

        return clients.Select(client => new ClientDto
        {
            Id = client.Id,
            Name = client.Name,
            Email = client.Email,
            Phone = client.Phone,
            IsActive = client.IsActive,
            ClientType = new ClientTypeDto
            {
                Id = client.ClientType.Id,
                Name = client.ClientType.Name,
                IsActive = client.ClientType.IsActive
            },
        });
    }
}
