using NightTasker.UserHub.Core.Application.Features.OrganizationUsers.Models;
using NightTasker.UserHub.Core.Domain.Entities;

namespace NightTasker.UserHub.Core.Application.Features.OrganizationUsers.Services.Contracts;

/// <summary>
/// Сервис для работы с пользователями организации (<see cref="OrganizationUser"/>).
/// </summary>
public interface IOrganizationUserService
{
    /// <summary>
    /// Создать пользователя организации.
    /// </summary>
    /// <param name="organizationUserDto">DTO для создания пользователя организации.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    Task CreateOrganizationUserWithOutSaving(
        CreateOrganizationUserDto organizationUserDto, CancellationToken cancellationToken);
}