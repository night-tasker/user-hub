using MediatR;
using NightTasker.UserHub.Core.Domain.Enums;

namespace NightTasker.UserHub.Core.Application.Features.OrganizationUsers.Queries.GetOrganizationUserRole;

/// <summary>
/// Запрос для получения роли пользователя в организации.
/// </summary>
/// <param name="OrganizationId">ИД организации.</param>
/// <param name="UserId">ИД пользователя.</param>
public record GetOrganizationUserRoleQuery(
    Guid OrganizationId, Guid UserId) : IRequest<OrganizationUserRole>;