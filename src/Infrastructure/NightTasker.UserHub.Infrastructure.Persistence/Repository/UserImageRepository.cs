using Microsoft.EntityFrameworkCore;
using NightTasker.Common.Core.Persistence;
using NightTasker.Common.Core.Persistence.Repository;
using NightTasker.UserHub.Core.Application.ApplicationContracts.Repository;
using NightTasker.UserHub.Core.Domain.Entities;

namespace NightTasker.UserHub.Infrastructure.Persistence.Repository;

/// <inheritdoc cref="NightTasker.UserHub.Core.Application.ApplicationContracts.Repository.IUserImageRepository" />
public class UserImageRepository(ApplicationDbSet<UserImage, Guid> dbSet) 
    : BaseRepository<UserImage, Guid>(dbSet), IUserImageRepository
{
    public Task<UserImage?> TryGetByUserInfoId(Guid userInfoId, bool trackChanges, CancellationToken cancellationToken)
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