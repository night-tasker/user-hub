using NightTasker.Common.Core.Persistence.Repository;
using NightTasker.UserHub.Core.Domain.Entities;

namespace NightTasker.UserHub.Core.Application.ApplicationContracts.Repository;

/// <summary>
/// Репозиторий для <see cref="UserInfo"/>
/// </summary>
public interface IUserInfoRepository : IRepository<UserInfo, Guid>
{
    
}