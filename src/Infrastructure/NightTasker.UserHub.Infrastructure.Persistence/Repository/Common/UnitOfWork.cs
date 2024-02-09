using NightTasker.UserHub.Core.Domain.Repositories;
using NightTasker.UserHub.Infrastructure.Persistence.Contracts;

namespace NightTasker.UserHub.Infrastructure.Persistence.Repository.Common;

public class UnitOfWork(IApplicationDataAccessor applicationDataAccessor) : IUnitOfWork
{
    public IUserRepository UserRepository { get; } = new UserRepository(applicationDataAccessor.Users);
    
    public IUserImageRepository UserImageRepository { get; } = new UserImageRepository(applicationDataAccessor.UserImages);
    
    public IOrganizationRepository OrganizationRepository { get; } = new OrganizationRepository(applicationDataAccessor.Organizations);

    public IOrganizationUserRepository OrganizationUserRepository { get; } =
        new OrganizationUserRepository(applicationDataAccessor.OrganizationUsers);

    public IOrganizationUserInviteRepository OrganizationUserInviteRepository { get; } =
        new OrganizationUserInviteRepository(applicationDataAccessor.OrganizationUserInvites);

    public Task SaveChanges(CancellationToken cancellationToken)
    {
        return applicationDataAccessor.SaveChanges(cancellationToken);
    }
}