namespace NightTasker.UserHub.Core.Application.ApplicationContracts.Repository;

public interface IUnitOfWork
{
    IUserInfoRepository UserInfoRepository { get; }
    
    IUserImageRepository UserImageRepository { get; }
    
    IOrganizationRepository OrganizationRepository { get; }
    
    IOrganizationUserRepository OrganizationUserRepository { get; }

    Task SaveChanges(CancellationToken cancellationToken);
}