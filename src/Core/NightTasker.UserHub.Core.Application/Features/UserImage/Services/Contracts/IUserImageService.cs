using NightTasker.UserHub.Core.Application.Features.UserImage.Models;

namespace NightTasker.UserHub.Core.Application.Features.UserImage.Services.Contracts;

/// <summary>
/// Сервис для работы с <see cref="UserImage"/>.
/// </summary>
public interface IUserImageService
{
    /// <summary>
    /// Создать <see cref="UserImage"/>.
    /// </summary>
    /// <param name="createUserImageDto">DTO для создания <see cref="UserImage"/>.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>ИД созданного <see cref="UserImage"/>.</returns>
    Task<Guid> CreateUserImage(CreateUserImageDto createUserImageDto, CancellationToken cancellationToken);

    /// <summary>
    /// Скачать <see cref="UserImage"/> по идентификатору пользователя.
    /// </summary>
    /// <param name="userInfoId">ИД пользователя.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Скачиваемый <see cref="UserImage"/>.</returns>
    Task<UserImageWithStreamDto> DownloadUserImageByUserInfoId(Guid userInfoId, CancellationToken cancellationToken);
    
    /// <summary>
    /// Получить ссылку на <see cref="UserImage"/> по идентификатору пользователя.
    /// </summary>
    /// <param name="userInfoId">ИД пользователя.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Ссылка на <see cref="UserImage"/>.</returns>
    Task<string> GetUserActiveImageUrlByUserInfoId(Guid userInfoId, CancellationToken cancellationToken);

    /// <summary>
    /// Получить ссылки на фотографии (<see cref="UserImage"/>) по идентификатору пользователя.
    /// </summary>
    /// <param name="userInfoId">ИД пользователя.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Ссылки на <see cref="UserImage"/>.</returns>
    Task<IReadOnlyCollection<UserImageWithUrlDto>> GetUserImagesByUserInfoId(
        Guid userInfoId, CancellationToken cancellationToken);
    
    /// <summary>
    /// Удалить <see cref="UserImage"/> по идентификатору.
    /// </summary>
    /// <param name="userImageId">ИД <see cref="UserImage"/>.</param>
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