using NightTasker.Common.Core.Persistence.Repository;
using NightTasker.UserHub.Core.Domain.Common.Search;
using NightTasker.UserHub.Core.Domain.Entities;

namespace NightTasker.UserHub.Core.Domain.Repositories;

public interface IOrganizationRepository : IRepository<Organization, Guid>
{
    Task<IReadOnlyCollection<Organization>> GetUserOrganizations(
        Guid userId, bool trackChanges, CancellationToken cancellationToken);

    Task<int> GetUsersCount(Guid organizationId, CancellationToken cancellationToken);

    Task<Organization?> TryGetOrganizationForUser(
        Guid userId, Guid organizationId, bool trackChanges, CancellationToken cancellationToken);

    Task<bool> CheckExistsById(Guid id, CancellationToken cancellationToken);

    Task<bool> CheckExistsByIdForUser(Guid userId, Guid id, CancellationToken cancellationToken);
    
    Task<SearchResult<Organization>> SearchOrganizationsForUser(
        Guid userId, ISearchCriteria<Organization> searchCriteria, CancellationToken cancellationToken);
}