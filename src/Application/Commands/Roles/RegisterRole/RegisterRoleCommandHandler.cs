using ClubApi.Application.Commands.Roles.Dtos;
using ClubApi.Domain.Abstractions;
using ClubApi.Domain.Entities;
using MediatR;

namespace ClubApi.Application.Commands.Roles.RegisterRole;

public class RegisterRoleCommandHandler : IRequestHandler<RegisterRoleCommand, RoleDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public RegisterRoleCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<RoleDto> Handle(RegisterRoleCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var role = Role.Create(request.Name);

            var roleCreated = await _unitOfWork.Repository<Role, int>().AddAsync(role);
            var roleDto = new RoleDto
            {
                Id = roleCreated.Id,
                Name = roleCreated.Name
            };

            return roleDto;
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw new InvalidOperationException("An error occurred while creating the user.", ex);
        }
    }
}