using ClubApi.Application.Commands.AccessLogs.Dtos;
using ClubApi.Application.Constants.Validations;
using ClubApi.Domain.Abstractions;
using ClubApi.Domain.Entities;
using MediatR;

namespace ClubApi.Application.Commands.AccessLogs.Entry;

public class RegisterEntryCommandHandler : IRequestHandler<RegisterEntryCommand, AccessLogDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public RegisterEntryCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<AccessLogDto> Handle(RegisterEntryCommand request, CancellationToken cancellationToken)
    {
        _ = await _unitOfWork.Repository<Client, int>().GetByIdAsync(request.ClientId, cancellationToken) ?? throw new KeyNotFoundException(string.Format(ValidationMessages.NOT_FOUND, "Cliente", request.ClientId));

        var accessLog = AccessLog.RegisterEntry(request.ClientId, request.EntryTime);

        var createdLog = await _unitOfWork.Repository<AccessLog, long>().AddAsync(accessLog, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new AccessLogDto
        {
            Id = createdLog.Id,
            ClientId = createdLog.ClientId,
            EntryTime = createdLog.EntryTime
        };
    }
}
