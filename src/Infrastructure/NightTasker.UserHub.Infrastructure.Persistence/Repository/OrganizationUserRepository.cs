using Microsoft.EntityFrameworkCore;
using NightTasker.Common.Core.Persistence;
using NightTasker.Common.Core.Persistence.Repository;
using NightTasker.UserHub.Core.Application.ApplicationContracts.Repository;
using NightTasker.UserHub.Core.Domain.Entities;
using NightTasker.UserHub.Core.Domain.Enums;

namespace NightTasker.UserHub.Infrastructure.Persistence.Repository;

public class OrganizationUserRepository(ApplicationDbSet<OrganizationUser, Guid> dbSet)
    : BaseRepository<OrganizationUser, Guid>(dbSet), IOrganizationUserRepository
{
    public async Task<OrganizationUserRole?> TryGetUserOrganizationRole(
        Guid organizationId, Guid userId, CancellationToken cancellationToken)
    {
        var organizationUser = await Entities
            .Where(x => x.OrganizationId == organizationId && x.UserId == userId)
            .SingleOrDefaultAsync(cancellationToken);
        
        return organizationUser?.Role;
    }
}