using NightTasker.UserHub.Core.Application.Features.UserInfo.Models;
using NightTasker.UserHub.Core.Domain.Entities;

namespace NightTasker.UserHub.Core.Application.Features.UserInfo.Services.Contracts;

/// <summary>
/// Сервис для работы с <see cref="UserInfo"/>.
/// </summary>
public interface IUserInfoService
{
    /// <summary>
    /// Создать <see cref="UserInfo"/>.
    /// </summary>
    /// <param name="createUserInfoDto"><see cref="CreateUserInfoDto"/></param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns></returns>
    Task CreateUserInfoWithSaving(CreateUserInfoDto createUserInfoDto, CancellationToken cancellationToken);
}