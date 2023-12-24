using Microsoft.EntityFrameworkCore;
using NightTasker.Common.Core.Persistence;
using NightTasker.Common.Core.Persistence.Repository;
using NightTasker.UserHub.Core.Application.ApplicationContracts.Repository;
using NightTasker.UserHub.Core.Domain.Entities;

namespace NightTasker.UserHub.Infrastructure.Persistence.Repository;

public class UserInfoRepository
    (ApplicationDbSet<UserInfo, Guid> dbSet) : BaseRepository<UserInfo, Guid>(dbSet), IUserInfoRepository
{
    /// <inheritdoc />
    public Task<UserInfo?> TryGetById(Guid id, CancellationToken cancellationToken)
    {
        return Entities.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
}