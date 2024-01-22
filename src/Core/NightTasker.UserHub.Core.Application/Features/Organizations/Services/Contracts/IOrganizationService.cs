using NightTasker.UserHub.Core.Application.Features.Organizations.Models;
using NightTasker.UserHub.Core.Domain.Entities;

namespace NightTasker.UserHub.Core.Application.Features.Organizations.Services.Contracts;

/// <summary>
/// Сервис для работы с организациями (<see cref="Organization"/>).
/// </summary>
public interface IOrganizationService
{
    /// <summary>
    /// Создать организацию.
    /// </summary>
    /// <param name="createOrganizationDto">DTO для создания организации.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Идентификатор созданной организации.</returns>
    Task<Guid> CreateOrganization(CreateOrganizationDto createOrganizationDto, CancellationToken cancellationToken);
}