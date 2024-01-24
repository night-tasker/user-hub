using NightTasker.UserHub.Core.Domain.Enums;

namespace NightTasker.UserHub.Core.Application.Features.OrganizationUsers.Models;

/// <summary>
/// DTO для создания пользователя организации.
/// </summary>
/// <param name="OrganizationId">Идентификатор организации.</param>
/// <param name="UserId">Идентификатор пользователя.</param>
/// <param name="Role">Роль пользователя в организации.</param>
public record CreateOrganizationUserDto(Guid OrganizationId, Guid UserId, OrganizationUserRole Role);