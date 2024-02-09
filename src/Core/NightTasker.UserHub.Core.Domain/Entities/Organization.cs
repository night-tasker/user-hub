﻿using NightTasker.Common.Core.Abstractions;
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

    public async Task<OrganizationUserInvite> SendInviteToUser(
        IOrganizationUserInviteRepository organizationUserInviteRepository,
        Guid inviterUserId, 
        Guid invitedUserId,
        string? message,
        CancellationToken cancellationToken)
    {
        var invite = OrganizationUserInvite.CreateInstance(inviterUserId, invitedUserId, Id, message);
        await organizationUserInviteRepository.Add(invite, cancellationToken);
        return invite;
    }

    public async Task<OrganizationUser> CreateAdmin(
        IOrganizationUserRepository organizationUserRepository,
        Guid userId,
        CancellationToken cancellationToken)
    {
        var organizationUser = OrganizationUser.CreateInstance(Id, userId, OrganizationUserRole.Admin);
        await organizationUserRepository.Add(organizationUser, cancellationToken);
        return organizationUser;
    }
}