using NightTasker.UserHub.Core.Application.Features.OrganizationUserInvites.Models;

namespace NightTasker.UserHub.Core.Application.Features.OrganizationUserInvites.Contracts;

public interface IOrganizationUserInviteService
{
    Task<Guid> SendOrganizationUserInvite(
        SendOrganizationUserInviteDto inviteDto, CancellationToken cancellationToken);
}