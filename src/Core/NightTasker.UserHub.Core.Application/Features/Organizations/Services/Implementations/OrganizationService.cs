﻿using MapsterMapper;
using NightTasker.UserHub.Core.Application.ApplicationContracts.Repository;
using NightTasker.UserHub.Core.Application.Features.Organizations.Models;
using NightTasker.UserHub.Core.Application.Features.Organizations.Services.Contracts;
using NightTasker.UserHub.Core.Domain.Entities;

namespace NightTasker.UserHub.Core.Application.Features.Organizations.Services.Implementations;

/// <inheritdoc />
public class OrganizationService(
    IUnitOfWork unitOfWork,
    IMapper mapper) : IOrganizationService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    /// <inheritdoc />
    public async Task<Guid> CreateOrganization(CreateOrganizationDto createOrganizationDto, CancellationToken cancellationToken)
    {
        var organization = _mapper.Map<Organization>(createOrganizationDto);
        await _unitOfWork.OrganizationRepository.Add(organization, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);
        return organization.Id;
    }
}