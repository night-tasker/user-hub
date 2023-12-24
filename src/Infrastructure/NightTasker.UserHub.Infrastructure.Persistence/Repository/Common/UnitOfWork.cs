using NightTasker.UserHub.Core.Application.ApplicationContracts.Repository;
using NightTasker.UserHub.Infrastructure.Persistence.Contracts;

namespace NightTasker.UserHub.Infrastructure.Persistence.Repository.Common;

public class UnitOfWork(IApplicationDbAccessor applicationDbAccessor) : IUnitOfWork
{
    public IUserInfoRepository UserInfoRepository { get; } = new UserInfoRepository(applicationDbAccessor.UserInfos);
    
    public Task SaveChanges(CancellationToken cancellationToken)
    {
        return applicationDbAccessor.SaveChanges(cancellationToken);
    }
}