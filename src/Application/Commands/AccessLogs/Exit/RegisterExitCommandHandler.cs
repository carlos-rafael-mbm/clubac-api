using ClubApi.Application.Commands.AccessLogs.Dtos;
using ClubApi.Application.Constants.Validations;
using ClubApi.Domain.Abstractions;
using ClubApi.Domain.Entities;
using MediatR;

namespace ClubApi.Application.Commands.AccessLogs.Exit;

public class RegisterExitCommandHandler : IRequestHandler<RegisterExitCommand, AccessLogDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public RegisterExitCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<AccessLogDto> Handle(RegisterExitCommand request, CancellationToken cancellationToken)
    {
        var accessLog = await _unitOfWork.Repository<AccessLog, long>().GetByIdAsync(request.AccessLogId, cancellationToken) ?? throw new KeyNotFoundException(string.Format(ValidationMessages.NOT_FOUND, "Log de Acceso", request.AccessLogId));

        if (accessLog.ExitTime != null)
        {
            throw new InvalidOperationException(ValidationMessages.EXIT_ALREADY_REGISTERED);
        }

        accessLog.RegisterExit(request.ExitTime);

        _unitOfWork.Repository<AccessLog, long>().Update(accessLog);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new AccessLogDto
        {
            Id = accessLog.Id,
            ClientId = accessLog.ClientId,
            EntryTime = accessLog.EntryTime,
            ExitTime = accessLog.ExitTime
        };
    }
}
