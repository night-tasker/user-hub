namespace NightTasker.UserHub.Core.Application.Features.OrganizationUserInvites.Models;

public record AcceptOrganizationUserInviteDto(Guid InviteId, Guid AcceptorUserId);