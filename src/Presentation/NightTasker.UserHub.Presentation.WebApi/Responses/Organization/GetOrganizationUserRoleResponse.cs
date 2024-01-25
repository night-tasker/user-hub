using NightTasker.UserHub.Core.Domain.Enums;

namespace NightTasker.UserHub.Presentation.WebApi.Responses.Organization;

/// <summary>
/// Ответ для получения роли пользователя в организации.
/// </summary>
public record GetOrganizationUserRoleResponse(OrganizationUserRole Role);