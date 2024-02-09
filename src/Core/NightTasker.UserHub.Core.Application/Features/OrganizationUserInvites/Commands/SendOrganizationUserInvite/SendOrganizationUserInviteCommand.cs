using MediatR;

namespace NightTasker.UserHub.Core.Application.Features.OrganizationUserInvites.Commands.SendOrganizationUserInvite;

public record SendOrganizationUserInviteCommand(
    Guid InviterUserId, Guid InvitedUserId, Guid OrganizationId, string? Message) : IRequest;