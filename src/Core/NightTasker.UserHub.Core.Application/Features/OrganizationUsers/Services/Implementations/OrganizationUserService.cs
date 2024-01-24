using MapsterMapper;
using NightTasker.UserHub.Core.Application.ApplicationContracts.Repository;
using NightTasker.UserHub.Core.Application.Features.OrganizationUsers.Models;
using NightTasker.UserHub.Core.Application.Features.OrganizationUsers.Services.Contracts;
using NightTasker.UserHub.Core.Domain.Entities;

namespace NightTasker.UserHub.Core.Application.Features.OrganizationUsers.Services.Implementations;

/// <inheritdoc />
public class OrganizationUserService(
    IUnitOfWork unitOfWork,
    IMapper mapper) : IOrganizationUserService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    /// <inheritdoc />
    public Task CreateOrganizationUserWithOutSaving(
        CreateOrganizationUserDto organizationUserDto, CancellationToken cancellationToken)
    {
        var organizationUser = _mapper.Map<OrganizationUser>(organizationUserDto);
        return _unitOfWork.OrganizationUserRepository.Add(organizationUser, cancellationToken);
    }
}