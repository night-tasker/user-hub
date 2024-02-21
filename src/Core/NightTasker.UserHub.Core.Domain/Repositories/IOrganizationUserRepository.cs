using NightTasker.Common.Core.Persistence.Repository;
using NightTasker.UserHub.Core.Domain.Entities;
using NightTasker.UserHub.Core.Domain.Enums;

namespace NightTasker.UserHub.Core.Domain.Repositories;

public interface IOrganizationUserRepository : IRepository<OrganizationUser, Guid>
{
    Task<OrganizationUserRole?> TryGetUserOrganizationRole(
        Guid organizationId, Guid userId, CancellationToken cancellationToken);

    Task<int> GetUsersCountInOrganization(Guid organizationId, CancellationToken cancellationToken);
    
    Task<OrganizationUser?> TryGetByOrganizationAndUserIds(
        Guid organizationId, Guid userId, CancellationToken cancellationToken);
}