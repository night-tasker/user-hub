using NightTasker.UserHub.Core.Domain.Entities;
using NightTasker.UserHub.Core.Domain.Repositories;

namespace NightTasker.UserHub.Core.Application.Models;

public record OrganizationWithInfoDto(
    Guid Id,
    string? Name,
    string? Description,
    int UsersCount,
    DateTimeOffset CreatedAt)
{
    public static async Task<OrganizationWithInfoDto> FromEntity(
        Organization organization, 
        IOrganizationUserRepository organizationUserRepository,
        CancellationToken cancellationToken)
    {
        return new OrganizationWithInfoDto(
            organization.Id,
            organization.Name,
            organization.Description,
            await organization.GetUsersCount(organizationUserRepository, cancellationToken),
            organization.CreatedDateTimeOffset);
    }
}