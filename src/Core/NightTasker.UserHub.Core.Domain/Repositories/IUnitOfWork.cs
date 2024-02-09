namespace NightTasker.UserHub.Core.Domain.Repositories;

public interface IUnitOfWork
{
    IUserRepository UserRepository { get; }
    
    IUserImageRepository UserImageRepository { get; }
    
    IOrganizationRepository OrganizationRepository { get; }
    
    IOrganizationUserRepository OrganizationUserRepository { get; }
    
    IOrganizationUserInviteRepository OrganizationUserInviteRepository { get; }

    Task SaveChanges(CancellationToken cancellationToken);
}