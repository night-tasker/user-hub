using NightTasker.Common.Core.Abstractions;
using NightTasker.UserHub.Core.Domain.Enums;
using NightTasker.UserHub.Core.Domain.Events.OrganizationUsers;
using NightTasker.UserHub.Core.Domain.Primitives;

namespace NightTasker.UserHub.Core.Domain.Entities;

public class OrganizationUser : AggregateRoot, IEntity
{
    private OrganizationUser(
        Guid organizationId,
        Guid userId,
        OrganizationUserRole role)
    {
        OrganizationId = organizationId;
        UserId = userId;
        Role = role;
    }

    public static OrganizationUser CreateInstance(
        Guid organizationId,
        Guid userId,
        OrganizationUserRole role)
    {
        var instance = new OrganizationUser(organizationId, userId, role);
        var domainEvent = new OrganizationUserCreatedDomainEvent(organizationId, userId);
        instance.RaiseDomainEvent(domainEvent);
        return instance;
    }
    
    public OrganizationUserRole Role { get; private set; }

    public Guid UserId { get; private set; }

    public User User { get; private set; } = null!;

    public Guid OrganizationId { get; private set; }

    public Organization Organization { get; private set; } = null!;
}