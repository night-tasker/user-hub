using NightTasker.UserHub.Core.Application.Exceptions.NotFound;
using NightTasker.UserHub.Core.Application.Exceptions.Unauthorized;
using NightTasker.UserHub.Core.Application.Features.Organizations.Models;
using NightTasker.UserHub.Core.Application.Features.Organizations.Services.Contracts;
using NightTasker.UserHub.Core.Domain.Entities;
using NightTasker.UserHub.Core.Domain.Enums;
using NightTasker.UserHub.Core.Domain.Repositories;

namespace NightTasker.UserHub.Core.Application.Features.Organizations.Services.Implementations;

internal class OrganizationService(
    IUnitOfWork unitOfWork) : IOrganizationService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

    public async Task<Guid> CreateOrganization(
        CreateOrganizationDto createOrganizationDto,
        CancellationToken cancellationToken)
    {
        var organization = createOrganizationDto.ToEntity();
        await _unitOfWork.OrganizationRepository.Add(organization, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);
        return organization.Id;
    }

    public async Task UpdateOrganizationAsUser(
        Guid userId,
        Guid organizationId,
        UpdateOrganizationDto updateOrganizationDto,
        CancellationToken cancellationToken)
    {
        var organization = await GetOrganizationForUser(userId, organizationId, cancellationToken);
        await ValidateUserHasAdminRoleInOrganization(organizationId, userId, cancellationToken);
        updateOrganizationDto.MapToEntityFields(organization);
        _unitOfWork.OrganizationRepository.Update(organization);
        await _unitOfWork.SaveChanges(cancellationToken);
    }

    private async Task<Organization> GetOrganizationForUser(
        Guid userId, Guid organizationId, CancellationToken cancellationToken)
    {
        var organization = await _unitOfWork.OrganizationRepository
            .TryGetOrganizationForUser(userId, organizationId, true, cancellationToken);
        
        if (organization is null)
        {
            throw new OrganizationUserNotFoundException(organizationId, userId);
        }
        
        return organization;
    }
    
    private async Task ValidateUserHasAdminRoleInOrganization(
        Guid organizationId, Guid userId, CancellationToken cancellationToken)
    {
        var organizationUserRole = await GetOrganizationUserRole(organizationId, userId, cancellationToken);
        if (organizationUserRole != OrganizationUserRole.Admin)
        {
            throw new UserCanNotUpdateOrganizationUnauthorizedException(organizationId, userId);
        }
    }

    private async Task<OrganizationUserRole> GetOrganizationUserRole(Guid organizationId, Guid userId, CancellationToken cancellationToken)
    {
        var organizationUserRole = await _unitOfWork.OrganizationUserRepository
            .TryGetUserOrganizationRole(organizationId, userId, cancellationToken);

        if (organizationUserRole is null)
        {
            throw new OrganizationUserNotFoundException(organizationId, userId);
        }
        
        return organizationUserRole.Value;
    }
}