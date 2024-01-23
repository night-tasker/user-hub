using NightTasker.Common.Core.Persistence.Repository;
using NightTasker.UserHub.Core.Domain.Entities;

namespace NightTasker.UserHub.Core.Application.ApplicationContracts.Repository;

/// <summary>
/// Репозиторий для <see cref="Organization"/>
/// </summary>
public interface IOrganizationRepository : IRepository<Organization, Guid>
{
    /// <summary>
    /// Получить организации пользователя.
    /// </summary>
    /// <param name="userInfoId">ИД пользователя.</param>
    /// <param name="trackChanges">Отслеживание изменений.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Список организаций.</returns>
    Task<IReadOnlyCollection<Organization>> GetUserOrganizations(
        Guid userInfoId, bool trackChanges, CancellationToken cancellationToken);   
}