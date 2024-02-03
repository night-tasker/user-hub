using NightTasker.Common.Core.Persistence.Repository;
using NightTasker.UserHub.Core.Domain.Entities;

namespace NightTasker.UserHub.Core.Domain.Repositories;

public interface IUserInfoRepository : IRepository<User, Guid>
{
    Task<User?> TryGetById(Guid id, bool trackChanges, CancellationToken cancellationToken);
    
    Task<bool> CheckExistsById(Guid id, CancellationToken cancellationToken);
}