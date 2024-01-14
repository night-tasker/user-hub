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
    /// Создать <see cref="UserImage"/> сохранением.
    /// </summary>
    /// <param name="createUserImageDto">DTO для создания <see cref="UserImage"/>.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>ИД созданного <see cref="UserImage"/>.</returns>
    Task<Guid> CreateUserImageWithSaving(CreateUserImageDto createUserImageDto, CancellationToken cancellationToken);
}