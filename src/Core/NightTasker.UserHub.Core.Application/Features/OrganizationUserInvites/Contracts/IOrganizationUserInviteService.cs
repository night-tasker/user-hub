using NightTasker.UserHub.Core.Application.Features.OrganizationUserInvites.Models;

namespace NightTasker.UserHub.Core.Application.Features.OrganizationUserInvites.Contracts;

public interface IOrganizationUserInviteService
{
    Task<Guid> SendInvite(
        SendOrganizationUserInviteDto inviteDto, CancellationToken cancellationToken);

    Task AcceptInvite(AcceptOrganizationUserInviteDto dto, CancellationToken cancellationToken);
    
    Task RevokeInvite(RevokeOrganizationUserInviteDto dto, CancellationToken cancellationToken);
}