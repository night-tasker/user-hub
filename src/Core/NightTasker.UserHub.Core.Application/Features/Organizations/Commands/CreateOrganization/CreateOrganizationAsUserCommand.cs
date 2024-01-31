using MediatR;

namespace NightTasker.UserHub.Core.Application.Features.Organizations.Commands.CreateOrganization;

public record CreateOrganizationAsUserCommand(string Name, string? Description, Guid UserId) : IRequest<Guid>;