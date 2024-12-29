using ClubApi.Application.Commands.Clients.Dtos;
using ClubApi.Application.Constants.Validations;
using ClubApi.Domain.Abstractions;
using ClubApi.Domain.Entities;
using MediatR;

namespace ClubApi.Application.Commands.Clients.RegisterClient;

public class RegisterClientCommandHandler : IRequestHandler<RegisterClientCommand, ClientDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public RegisterClientCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ClientDto> Handle(RegisterClientCommand request, CancellationToken cancellationToken)
    {
        var clientType = await _unitOfWork.Repository<ClientType, int>().GetByIdAsync(request.ClientTypeId, cancellationToken) ?? throw new KeyNotFoundException(string.Format(ValidationMessages.NOT_FOUND, "Tipo de cliente", request.ClientTypeId));

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var client = Client.Create(
                request.Name,
                request.ClientTypeId,
                request.Email,
                request.Phone
            );

            var clientCreated = await _unitOfWork.Repository<Client, int>().AddAsync(client, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            var clientDto = new ClientDto
            {
                Id = clientCreated.Id,
                Name = clientCreated.Name,
                Email = clientCreated.Email,
                Phone = clientCreated.Phone,
                IsActive = clientCreated.IsActive,
                ClientType = new ClientTypeDto
                {
                    Id = clientType.Id,
                    Name = clientType.Name,
                    IsActive = clientType.IsActive
                }
            };

            return clientDto;
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw new InvalidOperationException(ValidationMessages.INTERNAL_SERVER_ERROR, ex);
        }
    }
}
