using NightTasker.UserHub.Core.Application.Exceptions.NotFound;
using NightTasker.UserHub.Core.Application.Features.Organizations.Services.Contracts;
using NightTasker.UserHub.Core.Application.Features.OrganizationUserInvites.Contracts;
using NightTasker.UserHub.Core.Application.Features.OrganizationUserInvites.Models;
using NightTasker.UserHub.Core.Domain.Entities;
using NightTasker.UserHub.Core.Domain.Repositories;

namespace NightTasker.UserHub.Core.Application.Features.OrganizationUserInvites.Implementations;

internal sealed class OrganizationUserInviteService(
    IUnitOfWork unitOfWork,
    IOrganizationService organizationService) : IOrganizationUserInviteService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    private readonly IOrganizationService _organizationService = 
        organizationService ?? throw new ArgumentNullException(nameof(organizationService));
    
    public async Task<Guid> SendInvite(
        SendOrganizationUserInviteDto inviteDto, CancellationToken cancellationToken)
    {
        var organization = await _organizationService
            .GetOrganizationForUser(inviteDto.InviterUserId, inviteDto.OrganizationId, true, cancellationToken);
        await _organizationService.ValidateUserCanUpdateOrganization(
            inviteDto.OrganizationId, inviteDto.InviterUserId, cancellationToken);
        
        var invite = organization.SendInviteToUser(
            inviteDto.InviterUserId, 
            inviteDto.InvitedUserId, 
            inviteDto.Message);
        await _unitOfWork.OrganizationUserInviteRepository.Add(invite, cancellationToken);
        
        return invite.Id;
    }

    public async Task AcceptInvite(AcceptOrganizationUserInviteDto inviteDto, CancellationToken cancellationToken)
    {
        var invite = await GetInviteByIdForInvitedUser(inviteDto.InviteId, inviteDto.AcceptorUserId, cancellationToken);
        var organizationUser = invite.Accept();
        await _unitOfWork.OrganizationUserRepository.Add(organizationUser, cancellationToken);
    }

    public async Task RevokeInvite(RevokeOrganizationUserInviteDto dto, CancellationToken cancellationToken)
    {
        var invite = await GetInviteByIdForInvitedUser(dto.InviteId, dto.RevokerUserId, cancellationToken);
        invite.Revoke();
    }

    private async Task<OrganizationUserInvite> GetInviteByIdForInvitedUser(
        Guid inviteId, Guid invitedUserId, CancellationToken cancellationToken)
    {
        var invite = await _unitOfWork
            .OrganizationUserInviteRepository
            .TryGetIdForInvitedUser(inviteId, invitedUserId, cancellationToken);
        if (invite is null)
        {
            throw new OrganizationUserInviteForInvitedUserNotFoundException(inviteId, invitedUserId);
        }

        return invite;
    }
}