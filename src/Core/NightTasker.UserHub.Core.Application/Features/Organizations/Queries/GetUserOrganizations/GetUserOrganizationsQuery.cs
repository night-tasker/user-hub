using MediatR;
using NightTasker.UserHub.Core.Application.Features.Organizations.Models;

namespace NightTasker.UserHub.Core.Application.Features.Organizations.Queries.GetUserOrganizations;

/// <summary>
/// Запрос для получения организаций пользователя.
/// </summary>
/// <param name="UserInfoId">ИД пользователя.</param>
public record GetUserOrganizationsQuery(Guid UserInfoId) : IRequest<IReadOnlyCollection<OrganizationDto>>;