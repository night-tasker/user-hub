using MediatR;

namespace NightTasker.UserHub.Core.Application.Features.OrganizationUserInvites.Commands.RevokeInvite;

public sealed record RevokeOrganizationUserInviteCommand(Guid InviteId, Guid RevokerUserId) : IRequest;