using Microsoft.EntityFrameworkCore;
using NightTasker.Common.Core.Persistence;
using NightTasker.Common.Core.Persistence.Repository;
using NightTasker.UserHub.Core.Domain.Entities;
using NightTasker.UserHub.Core.Domain.Repositories;

namespace NightTasker.UserHub.Infrastructure.Persistence.Repository;

public class UserImageRepository(ApplicationDbSet<UserImage, Guid> dbSet) 
    : BaseRepository<UserImage, Guid>(dbSet), IUserImageRepository
{
    public Task<UserImage?> TryGetActiveImageByUserInfoId(
        Guid userInfoId, bool trackChanges, CancellationToken cancellationToken)
    {
        var entities = Entities
            .Where(x => x.IsActive);
        
        if (!trackChanges)
        {
            entities = entities.AsNoTracking();
        }
        
        return entities
            .SingleOrDefaultAsync(userImage => userImage.UserInfoId == userInfoId, cancellationToken);
    }

    public Task<UserImage?> TryGetImageByIdForUser(
        Guid userImageId, Guid userInfoId, bool trackChanges, CancellationToken cancellationToken)
    {
        var entities = Entities
            .Where(x => x.Id == userImageId && x.UserInfoId == userInfoId);
        
        if (!trackChanges)
        {
            entities = entities.AsNoTracking();
        }
        
        return entities
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<IDictionary<Guid, bool>> GetImageIdsWithActiveByUserInfoId(
        Guid userInfoId, CancellationToken cancellationToken)
    {
        var query = Entities
            .Where(x => x.UserInfoId == userInfoId);
        
        return await query
            .ToDictionaryAsync(x => x.Id, x => x.IsActive, cancellationToken);
    }

    public Task RemoveUserImageById(Guid userImageId, CancellationToken cancellationToken)
    {
        return Entities
            .Where(x => x.Id == userImageId)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public Task<bool> CheckImageForUserExists(Guid userInfoId, Guid userImageId, CancellationToken cancellationToken)
    {
        return Entities
            .AnyAsync(x => x.UserInfoId == userInfoId && x.Id == userImageId, cancellationToken);
    }

    public Task SetUnActiveImagesForUserInfoIdExcludeOne(
        Guid userInfoId,
        Guid activeUserImageId,
        CancellationToken cancellationToken)
    {
        return Entities
            .Where(x => x.UserInfoId == userInfoId && x.Id != activeUserImageId && x.IsActive)
            .ExecuteUpdateAsync(setPropertyCall => 
                setPropertyCall.SetProperty(prop => prop.IsActive, false), cancellationToken);
    }
}