using NightTasker.UserHub.Core.Application.Features.Organizations.Models;
using NightTasker.UserHub.Core.Domain.Entities;

namespace NightTasker.UserHub.Core.Application.Features.Organizations.Services.Contracts;

public interface IOrganizationService
{
    Task<Guid> CreateOrganizationAsUser(
        CreateOrganizationDto createOrganizationDto, Guid creatorUserId, CancellationToken cancellationToken);

    Task UpdateOrganizationAsUser(
        Guid userId,
        Guid organizationId,
        UpdateOrganizationDto updateOrganizationDto,
        CancellationToken cancellationToken);
    
    Task RemoveOrganizationAsUser(Guid userId, Guid organizationId, CancellationToken cancellationToken);

    Task<Organization> GetOrganizationForUser(
        Guid userId, Guid organizationId, bool trackChanges, CancellationToken cancellationToken);

    Task ValidateUserCanUpdateOrganization(
        Guid organizationId, Guid userId, CancellationToken cancellationToken);
}