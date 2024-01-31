using MapsterMapper;
using NightTasker.UserHub.Core.Application.ApplicationContracts.Repository;
using NightTasker.UserHub.Core.Application.Exceptions.NotFound;
using NightTasker.UserHub.Core.Application.Features.OrganizationUsers.Models;
using NightTasker.UserHub.Core.Application.Features.OrganizationUsers.Services.Contracts;
using NightTasker.UserHub.Core.Domain.Entities;

namespace NightTasker.UserHub.Core.Application.Features.OrganizationUsers.Services.Implementations;

public class OrganizationUserService(
    IUnitOfWork unitOfWork,
    IMapper mapper) : IOrganizationUserService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    public async Task CreateOrganizationUser(
        CreateOrganizationUserDto organizationUserDto, CancellationToken cancellationToken)
    {
        await ValidateUserInfoExists(organizationUserDto.UserId, cancellationToken);
        await ValidateOrganizationExists(organizationUserDto.OrganizationId, cancellationToken);

        var organizationUser = _mapper.Map<OrganizationUser>(organizationUserDto);
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