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
    Task<UserImageDto> DownloadUserImageByUserInfoId(Guid userInfoId, CancellationToken cancellationToken);
    
    /// <summary>
    /// Получить ссылку на <see cref="UserImage"/> по идентификатору пользователя.
    /// </summary>
    /// <param name="userInfoId">ИД пользователя.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Ссылка на <see cref="UserImage"/>.</returns>
    Task<string> GetUserImageUrlByUserInfoId(Guid userInfoId, CancellationToken cancellationToken);
}