using MediatR;
using NightTasker.UserHub.Core.Domain.Enums;

namespace NightTasker.UserHub.Core.Application.Features.OrganizationUsers.Queries.GetOrganizationUserRole;

public record GetOrganizationUserRoleQuery(
    Guid OrganizationId, Guid UserId) : IRequest<OrganizationUserRole>;