using Microsoft.EntityFrameworkCore;
using NightTasker.Common.Core.Persistence;
using NightTasker.Common.Core.Persistence.Repository;
using NightTasker.UserHub.Core.Domain.Entities;
using NightTasker.UserHub.Core.Domain.Repositories;

namespace NightTasker.UserHub.Infrastructure.Persistence.Repository;

public class OrganizationUserInviteRepository(ApplicationDbSet<OrganizationUserInvite, Guid> dbSet)
    : BaseRepository<OrganizationUserInvite, Guid>(dbSet), IOrganizationUserInviteRepository
{
    public async Task<OrganizationUserInvite?> TryGetByOrganization(Guid organizationId, CancellationToken cancellationToken)
    {
        var organizationUserInvite = await Entities
            .Where(x => x.OrganizationId == organizationId)
            .SingleOrDefaultAsync(cancellationToken);
        
        return organizationUserInvite;
    }
}