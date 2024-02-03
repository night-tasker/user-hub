using Microsoft.EntityFrameworkCore;
using NightTasker.Common.Core.Persistence;
using NightTasker.Common.Core.Persistence.Repository;
using NightTasker.UserHub.Core.Application.Models.Organization;
using NightTasker.UserHub.Core.Domain.Entities;
using NightTasker.UserHub.Core.Domain.Repositories;

namespace NightTasker.UserHub.Infrastructure.Persistence.Repository;

public class OrganizationRepository(ApplicationDbSet<Organization, Guid> dbSet)
    : BaseRepository<Organization, Guid>(dbSet), IOrganizationRepository
{
    public async Task<IReadOnlyCollection<Organization>> GetUserOrganizations(
        Guid userInfoId, bool trackChanges, CancellationToken cancellationToken)
    {
        var query = UserOrganizationsQuery(userInfoId);

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

    public async Task<OrganizationWithInfoDto?> TryGetOrganizationWithInfoForUser(Guid id, Guid userId, CancellationToken cancellationToken)
    {
        var query = UserOrganizationsQuery(userId)
            .Where(query => query.Id == id);

        var organization = await query
            .Select(x => new OrganizationWithInfoDto(
                x.Id, x.Name, x.Description, x.OrganizationUsers.Count))
            .SingleOrDefaultAsync(cancellationToken);
        
        return organization;
    }

    public Task<bool> CheckExistsByIdForUser(Guid userInfoId, Guid id, CancellationToken cancellationToken)
    {
        return Entities
            .AnyAsync(x => x.Id == id 
                           && x.OrganizationUsers.Any(y => y.UserId == userInfoId), cancellationToken);
    }
    
    public Task<bool> CheckExistsById(Guid id, CancellationToken cancellationToken)
    {
        return Entities
            .AnyAsync(x => x.Id == id, cancellationToken);
    }

    public Task<Organization?> TryGetOrganizationForUser(
        Guid userInfoId, Guid organizationId, bool trackChanges, CancellationToken cancellationToken)
    {
        var query = Entities
            .Where(x => x.Id == organizationId
                        && x.OrganizationUsers.Any(y => y.UserId == userInfoId));
        
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