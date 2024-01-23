using Microsoft.EntityFrameworkCore;
using NightTasker.Common.Core.Persistence;
using NightTasker.Common.Core.Persistence.Repository;
using NightTasker.UserHub.Core.Application.ApplicationContracts.Repository;
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
}