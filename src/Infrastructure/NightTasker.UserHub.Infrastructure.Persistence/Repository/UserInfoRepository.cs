using Microsoft.EntityFrameworkCore;
using NightTasker.Common.Core.Persistence;
using NightTasker.Common.Core.Persistence.Repository;
using NightTasker.UserHub.Core.Domain.Entities;
using NightTasker.UserHub.Core.Domain.Repositories;

namespace NightTasker.UserHub.Infrastructure.Persistence.Repository;

public class UserInfoRepository
    (ApplicationDbSet<UserInfo, Guid> dbSet) : BaseRepository<UserInfo, Guid>(dbSet), IUserInfoRepository
{
    public Task<UserInfo?> TryGetById(
        Guid id, 
        bool trackChanges, 
        CancellationToken cancellationToken)
    {
        var entities = Entities;
        if (!trackChanges)
        {
            entities = entities.AsNoTracking();
        }
        
        return entities
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task<bool> CheckExistsById(Guid id, CancellationToken cancellationToken)
    {
        return Entities
            .AnyAsync(x => x.Id == id, cancellationToken);
    }
}