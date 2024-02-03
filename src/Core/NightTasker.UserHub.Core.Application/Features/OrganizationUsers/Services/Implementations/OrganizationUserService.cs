using NightTasker.UserHub.Core.Application.Exceptions.NotFound;
using NightTasker.UserHub.Core.Application.Features.OrganizationUsers.Models;
using NightTasker.UserHub.Core.Application.Features.OrganizationUsers.Services.Contracts;
using NightTasker.UserHub.Core.Domain.Repositories;

namespace NightTasker.UserHub.Core.Application.Features.OrganizationUsers.Services.Implementations;

public class OrganizationUserService(
    IUnitOfWork unitOfWork) : IOrganizationUserService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

    public async Task CreateOrganizationUser(
        CreateOrganizationUserDto organizationUserDto, CancellationToken cancellationToken)
    {
        await ValidateUserInfoExists(organizationUserDto.UserId, cancellationToken);
        await ValidateOrganizationExists(organizationUserDto.OrganizationId, cancellationToken);

        var organizationUser = organizationUserDto.ToEntity();
        await _unitOfWork.OrganizationUserRepository.Add(organizationUser, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);
    }

    private async Task ValidateUserInfoExists(Guid userId, CancellationToken cancellationToken)
    {
        var userExists = await _unitOfWork.UserInfoRepository.CheckExistsById(userId, cancellationToken);
        if (!userExists)
        {
            throw new UserInfoNotFoundException(userId);
        }
    }

    private async Task ValidateOrganizationExists(Guid organizationId, CancellationToken cancellationToken)
    {
        var organizationExists = await _unitOfWork.OrganizationRepository
            .CheckExistsById(organizationId, cancellationToken);
        
        if (!organizationExists)
        {
            throw new OrganizationNotFoundException(organizationId);
        }
    }
}