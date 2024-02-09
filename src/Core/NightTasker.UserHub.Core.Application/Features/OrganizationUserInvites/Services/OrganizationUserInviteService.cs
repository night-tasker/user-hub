using NightTasker.UserHub.Core.Application.Features.Organizations.Services.Contracts;
using NightTasker.UserHub.Core.Application.Features.OrganizationUserInvites.Contracts;
using NightTasker.UserHub.Core.Application.Features.OrganizationUserInvites.Models;
using NightTasker.UserHub.Core.Domain.Repositories;

namespace NightTasker.UserHub.Core.Application.Features.OrganizationUserInvites.Services;

internal sealed class OrganizationUserInviteService(
    IUnitOfWork unitOfWork,
    IOrganizationService organizationService) : IOrganizationUserInviteService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    private readonly IOrganizationService _organizationService = 
        organizationService ?? throw new ArgumentNullException(nameof(organizationService));
    
    public async Task<Guid> SendOrganizationUserInvite(
        SendOrganizationUserInviteDto inviteDto, CancellationToken cancellationToken)
    {
        var organization = await _organizationService
            .GetOrganizationForUser(inviteDto.InviterUserId, inviteDto.OrganizationId, true, cancellationToken);
        await _organizationService.ValidateUserCanUpdateOrganization(
            inviteDto.OrganizationId, inviteDto.InviterUserId, cancellationToken);
        
        var invite = await organization.SendInviteToUser(
            _unitOfWork.OrganizationUserInviteRepository, 
            inviteDto.InviterUserId, 
            inviteDto.InvitedUserId, 
            inviteDto.Message, 
            cancellationToken);
        return invite.Id;
    }
}