using NightTasker.Common.Core.Exceptions.Base;

namespace NightTasker.UserHub.Core.Application.Exceptions.NotFound;

public class OrganizationUserInviteForInvitedUserNotFoundException(Guid inviteId, Guid invitedUserId)
    : NotFoundException($"Organization invite with id {inviteId} for invited user with id {invitedUserId} not found.");