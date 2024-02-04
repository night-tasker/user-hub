using Microsoft.EntityFrameworkCore;
using NightTasker.Common.Core.Persistence;
using NightTasker.Common.Core.Persistence.Repository;
using NightTasker.UserHub.Core.Domain.Common.Search;
using NightTasker.UserHub.Core.Domain.Entities;
using NightTasker.UserHub.Core.Domain.Repositories;

namespace NightTasker.UserHub.Infrastructure.Persistence.Repository;

public class OrganizationRepository(ApplicationDbSet<Organization, Guid> dbSet)
    : BaseRepository<Organization, Guid>(dbSet), IOrganizationRepository
{
    public async Task<IReadOnlyCollection<Organization>> GetUserOrganizations(
        Guid userId, bool trackChanges, CancellationToken cancellationToken)
    {
        var query = UserOrganizationsQuery(userId);

        if (!trackChanges)
        {
            query = query.AsNoTracking();
        }
        
        return await query.ToListAsync(cancellationToken);
    }

    public Task<int> GetUsersCount(Guid organizationId, CancellationToken cancellationToken)
    {
        return Entities
            .Where(x => x.Id == organizationId)
            .CountAsync(cancellationToken);
    }

    public Task<bool> CheckExistsByIdForUser(Guid userId, Guid id, CancellationToken cancellationToken)
    {
        return Entities
            .AnyAsync(x => x.Id == id 
                           && x.OrganizationUsers.Any(y => y.UserId == userId), cancellationToken);
    }

    public Task<SearchResult<Organization>> SearchOrganizationsForUser(
        Guid userId, ISearchCriteria<Organization> searchCriteria, CancellationToken cancellationToken)
    {
        var query = UserOrganizationsQuery(userId);
        return searchCriteria.Apply(query, cancellationToken);
    }

    public Task<bool> CheckExistsById(Guid id, CancellationToken cancellationToken)
    {
        return Entities
            .AnyAsync(x => x.Id == id, cancellationToken);
    }

    public Task<Organization?> TryGetOrganizationForUser(
        Guid userId, Guid organizationId, bool trackChanges, CancellationToken cancellationToken)
    {
        var query = Entities
            .Where(x => x.Id == organizationId
                        && x.OrganizationUsers.Any(y => y.UserId == userId));
        
        if (!trackChanges)
        {
            query = query.AsNoTracking();
        }
        
        return query
            .SingleOrDefaultAsync(cancellationToken);
    }

    private IQueryable<Organization> UserOrganizationsQuery(Guid userId)
    {
        return Entities
            .Where(x => x.OrganizationUsers.Any(y => y.UserId == userId));
    }
}