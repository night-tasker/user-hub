using NightTasker.Common.Core.Persistence;
using NightTasker.UserHub.Core.Domain.Entities;

namespace NightTasker.UserHub.Infrastructure.Persistence.Contracts;


public interface IApplicationDbAccessor
{
    ApplicationDbSet<User, Guid> UserInfos { get; }

    ApplicationDbSet<UserImage, Guid> UserImages { get; }

    ApplicationDbSet<Organization, Guid> Organizations { get; }

    ApplicationDbSet<OrganizationUser, Guid> OrganizationUsers { get; }

    Task SaveChanges(CancellationToken cancellationToken);
}