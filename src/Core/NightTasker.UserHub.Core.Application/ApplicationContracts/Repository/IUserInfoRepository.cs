using NightTasker.Common.Core.Persistence.Repository;
using NightTasker.UserHub.Core.Domain.Entities;

namespace NightTasker.UserHub.Core.Application.ApplicationContracts.Repository;

public interface IUserInfoRepository : IRepository<UserInfo, Guid>
{
    Task<UserInfo?> TryGetById(Guid id, bool trackChanges, CancellationToken cancellationToken);
    
    Task<bool> CheckExistsById(Guid id, CancellationToken cancellationToken);
}