using NightTasker.Common.Core.Abstractions;
using NightTasker.UserHub.Core.Domain.Enums;

namespace NightTasker.UserHub.Core.Domain.Entities;

public class OrganizationUserInvite : IEntityWithId<Guid>, IDateTimeOffsetModification
{
    private OrganizationUserInvite(
        Guid id,
        Guid inviterUserId,
        Guid invitedUserId,
        Guid organizationId,
        string? message)
    {
        Id = id;
        InviterUserId = inviterUserId;
        InvitedUserId = invitedUserId;
        OrganizationId = organizationId;
        Message = message;
    }

    public static OrganizationUserInvite CreateInstance(
        Guid inviterUserId, 
        Guid invitedUserId, 
        Guid organizationId, 
        string? message)
    {
        return new OrganizationUserInvite(Guid.NewGuid(), inviterUserId, invitedUserId, organizationId, message);
    }
    
    public static OrganizationUserInvite CreateInstance(
        Guid id,
        Guid inviterUserId, 
        Guid invitedUserId, 
        Guid organizationId, 
        string? message)
    {
        return new OrganizationUserInvite(id, inviterUserId, invitedUserId, organizationId, message);
    }

    public Guid Id { get; private set; }

    public Guid InvitedUserId { get; private set; }
    
    public User InvitedUser { get; private set; } = null!;
    
    public Guid OrganizationId { get; private set; }
    
    public Organization Organization { get; private set; } = null!;
    
    public Guid InviterUserId { get; private set; }
    
    public User InviterUser { get; private set; } = null!;
    
    public bool? IsAccepted { get; private set; }
    
    public bool? IsRevoked { get; private set; }
    
    public string? Message { get; private set; }
    
    public DateTimeOffset CreatedDateTimeOffset { get; set; }
    
    public DateTimeOffset? UpdatedDateTimeOffset { get; set; }

    public OrganizationUser Accept()
    {
        IsAccepted = true;
        return OrganizationUser.CreateInstance(OrganizationId, InvitedUserId, OrganizationUserRole.Member);
    }

    public void Revoke()
    {
        IsRevoked = true;
    }
}