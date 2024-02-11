using MediatR;
using NightTasker.UserHub.Core.Application.Features.OrganizationUserInvites.Contracts;
using NightTasker.UserHub.Core.Application.Features.OrganizationUserInvites.Models;
using NightTasker.UserHub.Core.Domain.Repositories;

namespace NightTasker.UserHub.Core.Application.Features.OrganizationUserInvites.Commands.SendInvite;

internal sealed class SendOrganizationUserInviteCommandHandler(
    IOrganizationUserInviteService organizationUserInviteService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<SendOrganizationUserInviteCommand>
{
    private readonly IOrganizationUserInviteService _organizationUserInviteService 
        = organizationUserInviteService ?? throw new ArgumentNullException(nameof(organizationUserInviteService));
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

    public async Task Handle(SendOrganizationUserInviteCommand request, CancellationToken cancellationToken)
    {
        var inviteDto = MapCommandToDto(request);
        await _organizationUserInviteService
            .SendInvite(inviteDto, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);
    }

    private static SendOrganizationUserInviteDto MapCommandToDto(SendOrganizationUserInviteCommand command)
    {
        return new SendOrganizationUserInviteDto(
            command.InviterUserId, command.InvitedUserId, command.OrganizationId, command.Message);
    }
}