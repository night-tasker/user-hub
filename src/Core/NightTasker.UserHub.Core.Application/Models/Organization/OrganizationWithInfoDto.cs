using NightTasker.UserHub.Core.Domain.Repositories;

namespace NightTasker.UserHub.Core.Application.Models.Organization;

public record OrganizationWithInfoDto(
    Guid Id,
    string? Name,
    string? Description,
    int UsersCount)
{
    public static async Task<OrganizationWithInfoDto> FromEntity(
        Domain.Entities.Organization organization, 
        IOrganizationUserRepository organizationUserRepository,
        CancellationToken cancellationToken)
    {
        return new OrganizationWithInfoDto(
            organization.Id,
            organization.Name,
            organization.Description,
            await organization.GetUsersCount(organizationUserRepository, cancellationToken));
    }
}