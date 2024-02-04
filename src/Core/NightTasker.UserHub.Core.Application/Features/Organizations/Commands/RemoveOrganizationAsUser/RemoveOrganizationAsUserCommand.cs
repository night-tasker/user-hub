using MediatR;

namespace NightTasker.UserHub.Core.Application.Features.Organizations.Commands.RemoveOrganizationAsUser;

public record RemoveOrganizationAsUserCommand(Guid UserId, Guid OrganizationId) : IRequest;