using NightTasker.UserHub.Core.Application.ApplicationContracts.Repository;

namespace NightTasker.UserHub.Infrastructure.Persistence.Repository.Common;

public class UnitOfWork(ApplicationDbContext dbContext) : IUnitOfWork
{
    public IUserInfoRepository UserInfoRepository { get; } = new UserInfoRepository(dbContext);
    
    public Task SaveChanges(CancellationToken cancellationToken)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}