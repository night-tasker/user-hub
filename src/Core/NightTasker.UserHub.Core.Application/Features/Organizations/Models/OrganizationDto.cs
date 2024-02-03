using NightTasker.UserHub.Core.Domain.Entities;

namespace NightTasker.UserHub.Core.Application.Features.Organizations.Models;

public record OrganizationDto(Guid Id, string? Name, string? Description)
{
    public static OrganizationDto FromEntity(Organization organization)
    {
        return new OrganizationDto(organization.Id, organization.Name, organization.Description);
    }

    public static IReadOnlyCollection<OrganizationDto> FromEntities(
        IReadOnlyCollection<Organization> organizations)
    {
        return organizations.Select(FromEntity).ToList();
    }
};