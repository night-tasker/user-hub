using NightTasker.Common.Core.Persistence.Repository;
using NightTasker.UserHub.Core.Domain.Entities;
using NightTasker.UserHub.Core.Domain.Enums;

namespace NightTasker.UserHub.Core.Application.ApplicationContracts.Repository;

/// <summary>
/// Репозиторий для работы с пользователями организации (<see cref="OrganizationUser"/>).
/// </summary>
public interface IOrganizationUserRepository : IRepository<OrganizationUser, Guid>
{
    /// <summary>
    /// Получить роль пользователя в организации.
    /// </summary>
    /// <param name="organizationId">ИД организации.</param>
    /// <param name="userId">ИД пользователя.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Список ролей.</returns>
    Task<OrganizationUserRole> GetUserOrganizationRole(
        Guid organizationId, Guid userId, CancellationToken cancellationToken);
};