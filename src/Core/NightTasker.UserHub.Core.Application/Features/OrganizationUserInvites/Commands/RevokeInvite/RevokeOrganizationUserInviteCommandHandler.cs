using MediatR;
using NightTasker.UserHub.Core.Application.Features.OrganizationUserInvites.Contracts;
using NightTasker.UserHub.Core.Application.Features.OrganizationUserInvites.Models;
using NightTasker.UserHub.Core.Domain.Repositories;

namespace NightTasker.UserHub.Core.Application.Features.OrganizationUserInvites.Commands.RevokeInvite;

internal sealed class RevokeOrganizationUserInviteCommandHandler(
    IOrganizationUserInviteService organizationUserInviteService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<RevokeOrganizationUserInviteCommand>
{
    private readonly IOrganizationUserInviteService _organizationUserInviteService = 
        organizationUserInviteService ?? throw new ArgumentNullException(nameof(organizationUserInviteService));
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    
    public async Task Handle(RevokeOrganizationUserInviteCommand request, CancellationToken cancellationToken)
    {
        var dto = MapCommandToDto(request);
        await _organizationUserInviteService
            .RevokeInvite(dto, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);
    }

    private RevokeOrganizationUserInviteDto MapCommandToDto(RevokeOrganizationUserInviteCommand request)
    {
        return new RevokeOrganizationUserInviteDto(request.InviteId, request.RevokerUserId);
    }
}