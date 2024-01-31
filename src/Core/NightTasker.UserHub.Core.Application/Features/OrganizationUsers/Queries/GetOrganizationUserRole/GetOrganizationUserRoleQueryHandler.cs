using MediatR;
using NightTasker.UserHub.Core.Application.ApplicationContracts.Repository;
using NightTasker.UserHub.Core.Application.Exceptions.NotFound;
using NightTasker.UserHub.Core.Domain.Enums;

namespace NightTasker.UserHub.Core.Application.Features.OrganizationUsers.Queries.GetOrganizationUserRole;

public class GetOrganizationUserRoleQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetOrganizationUserRoleQuery, OrganizationUserRole>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

    public Task<OrganizationUserRole> Handle(GetOrganizationUserRoleQuery request, CancellationToken cancellationToken)
    {
        return GetOrganizationUserRole(request.OrganizationId, request.UserId, cancellationToken);
    }

    private async Task<OrganizationUserRole> GetOrganizationUserRole(
        Guid organizationId, Guid userId, CancellationToken cancellationToken)
    {
        var organizationUserRole = await _unitOfWork.OrganizationUserRepository.TryGetUserOrganizationRole(
            organizationId, userId, cancellationToken);
        
        if(organizationUserRole is null)
        {
            throw new OrganizationUserNotFoundException(organizationId, userId);
        }
        
        return organizationUserRole.Value;
    }
}