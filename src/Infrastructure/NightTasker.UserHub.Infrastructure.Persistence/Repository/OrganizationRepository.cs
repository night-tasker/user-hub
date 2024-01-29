using Microsoft.EntityFrameworkCore;
using NightTasker.Common.Core.Persistence;
using NightTasker.Common.Core.Persistence.Repository;
using NightTasker.UserHub.Core.Application.ApplicationContracts.Repository;
using NightTasker.UserHub.Core.Application.Models.Organization;
using NightTasker.UserHub.Core.Domain.Entities;

namespace NightTasker.UserHub.Infrastructure.Persistence.Repository;

/// <inheritdoc cref="IOrganizationRepository"/>
public class OrganizationRepository(ApplicationDbSet<Organization, Guid> dbSet)
    : BaseRepository<Organization, Guid>(dbSet), IOrganizationRepository
{
    /// <inheritdoc />
    public async Task<IReadOnlyCollection<Organization>> GetUserOrganizations(
        Guid userInfoId, bool trackChanges, CancellationToken cancellationToken)
    {
        var query = Entities
            .Where(x => x.OrganizationUsers.Any(y => y.UserId == userInfoId));

        if (!trackChanges)
        {
            query = query.AsNoTracking();
        }
        
        return await query.ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public Task<Organization?> TryGetById(Guid id, bool trackChanges, CancellationToken cancellationToken)
    {
        var query = Entities
            .Where(x => x.Id == id);
        
        if (!trackChanges)
        {
            query = query.AsNoTracking();
        }
        
        return query.SingleOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<OrganizationWithInfoDto?> TryGetOrganizationWithInfo(Guid id, CancellationToken cancellationToken)
    {
        var query = Entities
            .Where(query => query.Id == id);

        var organization = await query
            .Select(x => new OrganizationWithInfoDto(
                x.Id, x.Name, x.Description, x.OrganizationUsers.Count))
            .SingleOrDefaultAsync(cancellationToken);
        
        return organization;
    }
}