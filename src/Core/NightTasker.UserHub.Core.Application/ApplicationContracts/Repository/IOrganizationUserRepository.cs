using NightTasker.Common.Core.Persistence.Repository;
using NightTasker.UserHub.Core.Domain.Entities;
using NightTasker.UserHub.Core.Domain.Enums;

namespace NightTasker.UserHub.Core.Application.ApplicationContracts.Repository;

public interface IOrganizationUserRepository : IRepository<OrganizationUser, Guid>
{
    Task<OrganizationUserRole?> TryGetUserOrganizationRole(
        Guid organizationId, Guid userId, CancellationToken cancellationToken);
}