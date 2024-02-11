using MediatR;

namespace NightTasker.UserHub.Core.Application.Features.OrganizationUserInvites.Commands.AcceptInvite;

public sealed record AcceptOrganizationUserInviteCommand(Guid InviteId, Guid AcceptorUserId) : IRequest;