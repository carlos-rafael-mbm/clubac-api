using ClubApi.Application.Queries.Clients.Dtos;
using ClubApi.Domain.Abstractions;
using ClubApi.Domain.Entities;
using MediatR;

namespace ClubApi.Application.Queries.Clients.GetAllClientType;

public class GetAllClientTypeQueryHandler : IRequestHandler<GetAllClientTypeQuery, IEnumerable<ClientTypeDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllClientTypeQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<ClientTypeDto>> Handle(GetAllClientTypeQuery request, CancellationToken cancellationToken)
    {
        return await Get(cancellationToken);
    }

    private async Task<IEnumerable<ClientTypeDto>> Get(CancellationToken cancellationToken)
    {
        var clientTypes = await _unitOfWork.Repository<ClientType, int>().GetAllAsync(cancellationToken);

        return clientTypes.Select(clientType => new ClientTypeDto
        {
            Id = clientType.Id,
            Name = clientType.Name,
            IsActive = clientType.IsActive,
        });
    }
}
