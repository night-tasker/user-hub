namespace NightTasker.UserHub.Core.Application.Features.OrganizationUserInvites.Models;

public record RevokeOrganizationUserInviteDto(Guid InviteId, Guid RevokerUserId);