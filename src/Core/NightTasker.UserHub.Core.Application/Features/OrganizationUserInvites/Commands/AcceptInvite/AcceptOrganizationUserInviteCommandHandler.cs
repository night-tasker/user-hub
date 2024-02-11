using MediatR;
using NightTasker.UserHub.Core.Application.Features.OrganizationUserInvites.Contracts;
using NightTasker.UserHub.Core.Application.Features.OrganizationUserInvites.Models;
using NightTasker.UserHub.Core.Domain.Repositories;

namespace NightTasker.UserHub.Core.Application.Features.OrganizationUserInvites.Commands.AcceptInvite;

internal sealed class AcceptOrganizationUserInviteCommandHandler(
    IOrganizationUserInviteService organizationUserInviteService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<AcceptOrganizationUserInviteCommand>
{
    private readonly IOrganizationUserInviteService _organizationUserInviteService = organizationUserInviteService ?? throw new ArgumentNullException(nameof(organizationUserInviteService));
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    
    public async Task Handle(AcceptOrganizationUserInviteCommand request, CancellationToken cancellationToken)
    {
        var dto = MapCommandToDto(request);
        await _organizationUserInviteService.AcceptInvite(dto, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);
    }

    private AcceptOrganizationUserInviteDto MapCommandToDto(AcceptOrganizationUserInviteCommand command)
    {
        return new AcceptOrganizationUserInviteDto(command.InviteId, command.AcceptorUserId);
    }
}