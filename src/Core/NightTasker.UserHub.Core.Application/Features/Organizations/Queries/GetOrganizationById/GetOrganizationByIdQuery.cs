using MediatR;
using NightTasker.UserHub.Core.Application.Features.Organizations.Models;

namespace NightTasker.UserHub.Core.Application.Features.Organizations.Queries.GetOrganizationById;

/// <summary>
/// Запрос для получения организации.
/// </summary>
/// 
public record GetOrganizationByIdQuery(Guid OrganizationId) : IRequest<OrganizationDto>;