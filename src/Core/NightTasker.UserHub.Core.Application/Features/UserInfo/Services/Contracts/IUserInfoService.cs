using NightTasker.UserHub.Core.Application.Features.UserInfo.Models;
using NightTasker.UserHub.Core.Domain.Entities;

namespace NightTasker.UserHub.Core.Application.Features.UserInfo.Services.Contracts;

/// <summary>
/// Сервис для работы с <see cref="UserInfo"/>.
/// </summary>
public interface IUserInfoService
{
    /// <summary>
    /// Получить <see cref="UserInfo"/> по ИД.
    /// </summary>
    /// <param name="userInfoId">ИД сведений о пользователе.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Сведения о пользователе.</returns>
    Task<UserInfoDto> GetUserInfoById(Guid userInfoId, CancellationToken cancellationToken);
    
    /// <summary>
    /// Создать <see cref="UserInfo"/>.
    /// </summary>
    /// <param name="createUserInfoDto"><see cref="CreateUserInfoDto"/></param>
    /// <param name="cancellationToken">Токен отмены.</param>
    Task CreateUserInfoWithSaving(CreateUserInfoDto createUserInfoDto, CancellationToken cancellationToken);

    /// <summary>
    /// Обновить <see cref="UserInfo"/>
    /// </summary>
    /// <param name="updateUserInfoDto"><see cref="UpdateUserInfoDto"/></param>
    /// <param name="cancellationToken">Токен отмены.</param>
    Task UpdateUserInfoWithSaving(UpdateUserInfoDto updateUserInfoDto, CancellationToken cancellationToken);
}