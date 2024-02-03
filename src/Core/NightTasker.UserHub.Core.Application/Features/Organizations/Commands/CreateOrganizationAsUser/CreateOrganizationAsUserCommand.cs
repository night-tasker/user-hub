using MediatR;
using NightTasker.UserHub.Core.Application.Features.Organizations.Models;

namespace NightTasker.UserHub.Core.Application.Features.Organizations.Commands.CreateOrganizationAsUser;

public record CreateOrganizationAsUserCommand(string Name, string? Description, Guid UserId) : IRequest<Guid>
{
    public CreateOrganizationDto ToCreateOrganizationDto()
    {
        return new CreateOrganizationDto(Name, Description);
    }
}