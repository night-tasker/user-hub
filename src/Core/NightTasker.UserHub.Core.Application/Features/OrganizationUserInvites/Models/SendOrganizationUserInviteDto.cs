using NightTasker.UserHub.Core.Domain.Entities;

namespace NightTasker.UserHub.Core.Application.Features.OrganizationUserInvites.Models;

public sealed record SendOrganizationUserInviteDto(
    Guid InviterUserId,
    Guid InvitedUserId,
    Guid OrganizationId,
    string? Message)
{
    public OrganizationUserInvite ToEntity() => OrganizationUserInvite.CreateInstance(
        InviterUserId,
        InvitedUserId, 
        OrganizationId, 
        Message); 
}