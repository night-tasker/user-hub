namespace NightTasker.UserHub.Core.Application.ApplicationContracts.Repository;

/// <summary>
/// Unit of work.
/// </summary>
public interface IUnitOfWork
{
    /// <inheritdoc cref="IUserInfoRepository"/>
    IUserInfoRepository UserInfoRepository { get; }
    
    /// <inheritdoc cref="IUserImageRepository"/>
    IUserImageRepository UserImageRepository { get; }

    Task SaveChanges(CancellationToken cancellationToken);
}