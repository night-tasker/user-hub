using NightTasker.Common.Core.Persistence.Repository;
using NightTasker.UserHub.Core.Domain.Entities;

namespace NightTasker.UserHub.Core.Domain.Repositories;

public interface IOrganizationUserInviteRepository : IRepository<OrganizationUserInvite, Guid>
{
    Task<OrganizationUserInvite?> TryGetIdForInvitedUser(
        Guid inviteId, Guid invitedUserId, CancellationToken cancellationToken);
}