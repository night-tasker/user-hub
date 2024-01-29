using MediatR;
using NightTasker.UserHub.Core.Application.Features.Organizations.Models;
using NightTasker.UserHub.Core.Application.Models.Organization;

namespace NightTasker.UserHub.Core.Application.Features.Organizations.Queries.GetOrganizationById;

/// <summary>
/// Запрос для получения организации.
/// </summary>
/// 
public record GetOrganizationByIdQuery(Guid OrganizationId) : IRequest<OrganizationWithInfoDto>;