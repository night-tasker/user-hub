using NightTasker.Common.Core.Persistence.Repository;
using NightTasker.UserHub.Core.Application.Models.Organization;
using NightTasker.UserHub.Core.Domain.Entities;

namespace NightTasker.UserHub.Core.Application.ApplicationContracts.Repository;

public interface IOrganizationRepository : IRepository<Organization, Guid>
{
    Task<IReadOnlyCollection<Organization>> GetUserOrganizations(
        Guid userInfoId, bool trackChanges, CancellationToken cancellationToken);

    Task<OrganizationWithInfoDto?> TryGetOrganizationWithInfoForUser(Guid id, Guid userId, CancellationToken cancellationToken);

    Task<bool> CheckExistsById(Guid id, CancellationToken cancellationToken);

    Task<bool> CheckExistsByIdForUser(Guid userInfoId, Guid id, CancellationToken cancellationToken);
}