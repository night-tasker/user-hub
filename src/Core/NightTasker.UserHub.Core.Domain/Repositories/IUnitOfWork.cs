namespace NightTasker.UserHub.Core.Domain.Repositories;

public interface IUnitOfWork
{
    IUserInfoRepository UserInfoRepository { get; }
    
    IUserImageRepository UserImageRepository { get; }
    
    IOrganizationRepository OrganizationRepository { get; }
    
    IOrganizationUserRepository OrganizationUserRepository { get; }

    Task SaveChanges(CancellationToken cancellationToken);
}