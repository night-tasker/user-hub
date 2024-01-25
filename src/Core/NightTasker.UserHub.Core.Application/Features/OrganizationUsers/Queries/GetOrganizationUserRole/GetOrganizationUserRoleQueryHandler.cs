using MediatR;
using NightTasker.UserHub.Core.Application.ApplicationContracts.Repository;
using NightTasker.UserHub.Core.Domain.Enums;

namespace NightTasker.UserHub.Core.Application.Features.OrganizationUsers.Queries.GetOrganizationUserRole;

/// <summary>
/// Хэндлер для <see cref="GetOrganizationUserRoleQuery"/>
/// </summary>
public class GetOrganizationUserRoleQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetOrganizationUserRoleQuery, OrganizationUserRole>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

    public Task<OrganizationUserRole> Handle(GetOrganizationUserRoleQuery request, CancellationToken cancellationToken)
    {
        return _unitOfWork.OrganizationUserRepository.GetUserOrganizationRole(
            request.OrganizationId, request.UserId, cancellationToken);
    }
}