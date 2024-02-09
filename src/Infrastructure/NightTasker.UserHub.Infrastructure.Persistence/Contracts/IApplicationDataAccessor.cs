using NightTasker.Common.Core.Persistence;
using NightTasker.UserHub.Core.Domain.Entities;

namespace NightTasker.UserHub.Infrastructure.Persistence.Contracts;

public interface IApplicationDataAccessor
{
    ApplicationDbSet<User, Guid> Users { get; }

    ApplicationDbSet<UserImage, Guid> UserImages { get; }

    ApplicationDbSet<Organization, Guid> Organizations { get; }

    ApplicationDbSet<OrganizationUser, Guid> OrganizationUsers { get; }
    
    ApplicationDbSet<OrganizationUserInvite, Guid> OrganizationUserInvites { get; }

    Task SaveChanges(CancellationToken cancellationToken);
}