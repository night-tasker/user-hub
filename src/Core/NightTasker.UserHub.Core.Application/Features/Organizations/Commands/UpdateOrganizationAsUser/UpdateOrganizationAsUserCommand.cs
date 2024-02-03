using MediatR;
using NightTasker.UserHub.Core.Application.Features.Organizations.Models;

namespace NightTasker.UserHub.Core.Application.Features.Organizations.Commands.UpdateOrganizationAsUser;

public record UpdateOrganizationAsUserCommand(
    Guid UserId, Guid OrganizationId, UpdateOrganizationDto UpdateOrganizationDto) : IRequest;