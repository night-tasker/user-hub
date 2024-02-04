using MediatR;
using NightTasker.UserHub.Core.Application.Features.Organizations.Models;
using NightTasker.UserHub.Core.Application.Models;

namespace NightTasker.UserHub.Core.Application.Features.Organizations.Queries.GetOrganizationById;

public record GetOrganizationByIdAsUserQuery(Guid OrganizationId, Guid UserId) : IRequest<OrganizationWithInfoDto>;