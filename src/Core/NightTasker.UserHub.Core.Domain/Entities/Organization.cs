using NightTasker.Common.Core.Abstractions;
using NightTasker.UserHub.Core.Domain.Enums;
using NightTasker.UserHub.Core.Domain.Repositories;

namespace NightTasker.UserHub.Core.Domain.Entities;

public class Organization : IEntityWithId<Guid>, IDateTimeOffsetModification
{
    private Organization(
        Guid id,
        string? name,
        string? description)
    {
        Id = id;
        Name = name;
        Description = description;
    }
    
    public static Organization CreateInstance(
        string? name,
        string? description)
    {
        return new Organization(Guid.NewGuid(), name, description);
    }
    
    public static Organization CreateInstance(
        Guid id,
        string? name,
        string? description)
    {
        return new Organization(id, name, description);
    }
    
    public Guid Id { get; private set; }

    public string? Name { get; private set; }

    public string? Description { get; private set; }

    public IReadOnlyCollection<OrganizationUser> OrganizationUsers { get; } = null!;
    
    public IReadOnlyCollection<OrganizationUserInvite> OrganizationUserInvites { get; } = null!;

    public DateTimeOffset CreatedDateTimeOffset { get; set; }
    
    public DateTimeOffset? UpdatedDateTimeOffset { get; set; }

    public Task<int> GetUsersCount(IOrganizationUserRepository organizationRepository, CancellationToken cancellationToken)
    {
        return organizationRepository.GetUsersCountInOrganization(Id, cancellationToken);
    }

    public void UpdateName(string name)
    {
        Name = name;
    }

    public void UpdateDescription(string description)
    {
        Description = description;
    }

    public OrganizationUserInvite SendInviteToUser(
        Guid inviterUserId, 
        Guid invitedUserId,
        string? message)
    {
        var invite = OrganizationUserInvite.CreateInstance(inviterUserId, invitedUserId, Id, message);
        return invite;
    }

    public OrganizationUser CreateAdmin(Guid userId)
    {
        var organizationUser = OrganizationUser.CreateInstance(Id, userId, OrganizationUserRole.Admin);
        return organizationUser;
    }
}