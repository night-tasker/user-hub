using NightTasker.Common.Core.Persistence.Repository;
using NightTasker.UserHub.Core.Domain.Entities;

namespace NightTasker.UserHub.Core.Application.ApplicationContracts.Repository;

/// <summary>
/// Репозиторий для <see cref="UserImage"/>
/// </summary>
public interface IUserImageRepository : IRepository<UserImage, Guid>
{
    /// <summary>
    /// Получить <see cref="UserImage"/> по идентификатору пользователя.
    /// </summary>
    /// <param name="userInfoId">ИД пользователя.</param>
    /// <param name="trackChanges">Отслеживать изменения.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>
    /// Фото пользователя.
    /// В случае отсутствия фото у пользователя вернет null.
    /// </returns>
    Task<UserImage?> TryGetByUserInfoId(Guid userInfoId, bool trackChanges, CancellationToken cancellationToken);

    /// <summary>
    /// Установить не активные фото пользователя кроме одного.
    /// </summary>
    /// <param name="userInfoId">ИД пользователя.</param>
    /// <param name="activeUserImageId">ИД активного фото пользователя.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns></returns>
    Task SetUnActiveImagesForUserInfoIdExcludeOne(
        Guid userInfoId,
        Guid activeUserImageId,
        CancellationToken cancellationToken);
}