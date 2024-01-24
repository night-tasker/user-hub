using MapsterMapper;
using MediatR;
using NightTasker.UserHub.Core.Application.ApplicationContracts.Repository;
using NightTasker.UserHub.Core.Application.Features.Organizations.Models;
using NightTasker.UserHub.Core.Application.Features.Organizations.Services.Contracts;
using NightTasker.UserHub.Core.Application.Features.OrganizationUsers.Models;
using NightTasker.UserHub.Core.Application.Features.OrganizationUsers.Services.Contracts;
using NightTasker.UserHub.Core.Domain.Enums;

namespace NightTasker.UserHub.Core.Application.Features.Organizations.Commands.CreateOrganization;

/// <summary>
/// Хэндлер для <see cref="CreateOrganizationAsUserCommand"/>
/// </summary>
internal class CreateOrganizationAsUserCommandHandler(
    IOrganizationService organizationService,
    IOrganizationUserService organizationUserService,
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : IRequestHandler<CreateOrganizationAsUserCommand, Guid>
{
    private readonly IOrganizationService _organizationService = 
        organizationService ?? throw new ArgumentNullException(nameof(organizationService));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    private readonly IOrganizationUserService _organizationUserService = 
        organizationUserService ?? throw new ArgumentNullException(nameof(organizationUserService));
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

    public async Task<Guid> Handle(CreateOrganizationAsUserCommand request, CancellationToken cancellationToken)
    {
        var createOrganizationDto = _mapper.Map<CreateOrganizationDto>(request);
        var organizationId = await _organizationService.CreateOrganizationWithOutSaving(
            createOrganizationDto, cancellationToken);
        
        var createOrganizationUserDto = new CreateOrganizationUserDto(
            organizationId, request.UserId, OrganizationUserRole.Admin);
        await _organizationUserService.CreateOrganizationUserWithOutSaving(createOrganizationUserDto, cancellationToken);
        
        await _unitOfWork.SaveChanges(cancellationToken);
        return organizationId;
    }
}