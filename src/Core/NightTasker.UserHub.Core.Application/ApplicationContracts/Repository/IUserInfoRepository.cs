using NightTasker.Common.Core.Persistence.Repository;
using NightTasker.UserHub.Core.Domain.Entities;

namespace NightTasker.UserHub.Core.Application.ApplicationContracts.Repository;

/// <summary>
/// Репозиторий для <see cref="UserInfo"/>
/// </summary>
public interface IUserInfoRepository : IRepository<UserInfo, Guid>
{
    /// <summary>
    /// Получить <see cref="UserInfo"/> по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <param name="trackChanges">Отслеживать изменения.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns><see cref="UserInfo"/></returns>
    Task<UserInfo?> TryGetById(Guid id, bool trackChanges, CancellationToken cancellationToken);
}