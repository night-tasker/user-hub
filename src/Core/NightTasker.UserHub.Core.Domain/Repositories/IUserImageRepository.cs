using NightTasker.Common.Core.Persistence.Repository;
using NightTasker.UserHub.Core.Domain.Entities;

namespace NightTasker.UserHub.Core.Domain.Repositories;

public interface IUserImageRepository : IRepository<UserImage, Guid>
{
    Task<UserImage?> TryGetActiveImageByUserInfoId(Guid userInfoId, bool trackChanges,
        CancellationToken cancellationToken);

    Task<UserImage?> TryGetImageByIdForUser(Guid userImageId, Guid userInfoId, bool trackChanges,
        CancellationToken cancellationToken);

    Task SetUnActiveImagesForUserInfoIdExcludeOne(
        Guid userInfoId,
        Guid activeUserImageId,
        CancellationToken cancellationToken);

    Task<IDictionary<Guid, bool>> GetImageIdsWithActiveByUserInfoId(
        Guid userInfoId, CancellationToken cancellationToken);

    Task RemoveUserImageById(Guid userImageId, CancellationToken cancellationToken);

    Task<bool> CheckImageForUserExists(Guid userInfoId, Guid userImageId, CancellationToken cancellationToken);
}