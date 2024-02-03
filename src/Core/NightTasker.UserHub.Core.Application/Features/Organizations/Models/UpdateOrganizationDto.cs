using NightTasker.UserHub.Core.Domain.Entities;

namespace NightTasker.UserHub.Core.Application.Features.Organizations.Models;

public record UpdateOrganizationDto(string Name, string Description)
{
    public Organization MapToEntityFields(Organization organization)
    {
        organization.UpdateName(Name);
        organization.UpdateDescription(Description);
        return organization;
    }
};