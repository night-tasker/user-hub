using NightTasker.UserHub.Core.Application.Features.UserImages.Models;

namespace NightTasker.UserHub.Core.Application.Features.UserImages.Services.Contracts;

/// <summary>
/// Сервис для работы с <see cref="UserImages"/>.
/// </summary>
public interface IUserImageService
{
    /// <summary>
    /// Создать <see cref="UserImages"/>.
    /// </summary>
    /// <param name="createUserImageDto">DTO для создания <see cref="UserImages"/>.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>ИД созданного <see cref="UserImages"/>.</returns>
    Task<Guid> CreateUserImage(CreateUserImageDto createUserImageDto, CancellationToken cancellationToken);

    /// <summary>
    /// Скачать <see cref="UserImages"/> по идентификатору пользователя.
    /// </summary>
    /// <param name="userInfoId">ИД пользователя.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Скачиваемый <see cref="UserImages"/>.</returns>
    Task<UserImageWithStreamDto> DownloadUserImageByUserInfoId(Guid userInfoId, CancellationToken cancellationToken);
    
    /// <summary>
    /// Получить ссылку на <see cref="UserImages"/> по идентификатору пользователя.
    /// </summary>
    /// <param name="userInfoId">ИД пользователя.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Ссылка на <see cref="UserImages"/>.</returns>
    Task<string> GetUserActiveImageUrlByUserInfoId(Guid userInfoId, CancellationToken cancellationToken);

    /// <summary>
    /// Получить ссылки на фотографии (<see cref="UserImages"/>) по идентификатору пользователя.
    /// </summary>
    /// <param name="userInfoId">ИД пользователя.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Ссылки на <see cref="UserImages"/>.</returns>
    Task<IReadOnlyCollection<UserImageWithUrlDto>> GetUserImagesByUserInfoId(
        Guid userInfoId, CancellationToken cancellationToken);
    
    /// <summary>
    /// Удалить <see cref="UserImages"/> по идентификатору.
    /// </summary>
    /// <param name="userImageId">ИД <see cref="UserImages"/>.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    Task RemoveUserImageById(Guid userImageId, CancellationToken cancellationToken);

    /// <summary>
    /// Установить активную фотографию для пользователя.
    /// </summary>
    /// <param name="userInfoId">ИД пользователя.</param>
    /// <param name="userImageId">ИД фотографии.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <exception cref="UserImageWithIdNotFoundException"></exception>
    Task SetActiveUserImageForUserInfoId(
        Guid userInfoId, Guid userImageId, CancellationToken cancellationToken);
}