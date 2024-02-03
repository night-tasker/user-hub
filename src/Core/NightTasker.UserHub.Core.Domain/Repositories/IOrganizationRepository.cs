using NightTasker.Common.Core.Persistence.Repository;
using NightTasker.UserHub.Core.Domain.Entities;

namespace NightTasker.UserHub.Core.Domain.Repositories;

public interface IOrganizationRepository : IRepository<Organization, Guid>
{
    Task<IReadOnlyCollection<Organization>> GetUserOrganizations(
        Guid userInfoId, bool trackChanges, CancellationToken cancellationToken);

    Task<int> GetUsersCount(Guid organizationId, CancellationToken cancellationToken);

    Task<Organization?> TryGetOrganizationForUser(
        Guid userInfoId, Guid organizationId, bool trackChanges, CancellationToken cancellationToken);

    Task<bool> CheckExistsById(Guid id, CancellationToken cancellationToken);

    Task<bool> CheckExistsByIdForUser(Guid userInfoId, Guid id, CancellationToken cancellationToken);
}