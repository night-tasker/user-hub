using NightTasker.Common.Core.Persistence.Repository;
using NightTasker.UserHub.Core.Domain.Entities;

namespace NightTasker.UserHub.Core.Domain.Repositories;

public interface IUserImageRepository : IRepository<UserImage, Guid>
{
    Task<UserImage?> TryGetActiveImageByUserId(Guid userId, bool trackChanges,
        CancellationToken cancellationToken);

    Task<UserImage?> TryGetImageByIdForUser(Guid userImageId, Guid userId, bool trackChanges,
        CancellationToken cancellationToken);

    Task SetUnActiveImagesForUserIdExcludeOne(
        Guid userId,
        Guid activeUserImageId,
        CancellationToken cancellationToken);

    Task<IDictionary<Guid, bool>> GetImageIdsWithActiveByUserId(
        Guid userId, CancellationToken cancellationToken);

    Task RemoveUserImageById(Guid userImageId, CancellationToken cancellationToken);

    Task<bool> CheckImageForUserExists(Guid userId, Guid userImageId, CancellationToken cancellationToken);
}