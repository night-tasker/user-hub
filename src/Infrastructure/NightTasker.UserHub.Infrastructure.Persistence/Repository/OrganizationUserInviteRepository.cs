using Microsoft.EntityFrameworkCore;
using NightTasker.Common.Core.Persistence;
using NightTasker.Common.Core.Persistence.Repository;
using NightTasker.UserHub.Core.Domain.Entities;
using NightTasker.UserHub.Core.Domain.Repositories;

namespace NightTasker.UserHub.Infrastructure.Persistence.Repository;

public class OrganizationUserInviteRepository(ApplicationDbSet<OrganizationUserInvite, Guid> dbSet)
    : BaseRepository<OrganizationUserInvite, Guid>(dbSet), IOrganizationUserInviteRepository
{
    public Task<OrganizationUserInvite?> TryGetIdForInvitedUser(
        Guid inviteId, Guid invitedUserId, CancellationToken cancellationToken)
    {
        return Entities
            .SingleOrDefaultAsync(x => x.Id == inviteId && x.InvitedUserId == invitedUserId, cancellationToken);
    }
}