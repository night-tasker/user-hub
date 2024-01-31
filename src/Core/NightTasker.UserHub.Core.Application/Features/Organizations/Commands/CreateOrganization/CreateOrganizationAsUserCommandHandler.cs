using MapsterMapper;
using MediatR;
using NightTasker.UserHub.Core.Application.Features.Organizations.Models;
using NightTasker.UserHub.Core.Application.Features.Organizations.Services.Contracts;
using NightTasker.UserHub.Core.Application.Features.OrganizationUsers.Models;
using NightTasker.UserHub.Core.Application.Features.OrganizationUsers.Services.Contracts;
using NightTasker.UserHub.Core.Domain.Enums;

namespace NightTasker.UserHub.Core.Application.Features.Organizations.Commands.CreateOrganization;

internal class CreateOrganizationAsUserCommandHandler(
    IOrganizationService organizationService,
    IOrganizationUserService organizationUserService,
    IMapper mapper)
    : IRequestHandler<CreateOrganizationAsUserCommand, Guid>
{
    private readonly IOrganizationService _organizationService = 
        organizationService ?? throw new ArgumentNullException(nameof(organizationService));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    private readonly IOrganizationUserService _organizationUserService = 
        organizationUserService ?? throw new ArgumentNullException(nameof(organizationUserService));

    public async Task<Guid> Handle(CreateOrganizationAsUserCommand request, CancellationToken cancellationToken)
    {
        var createOrganizationDto = _mapper.Map<CreateOrganizationDto>(request);
        var organizationId = await _organizationService.CreateOrganization(
            createOrganizationDto, cancellationToken);
        
        var createOrganizationUserDto = new CreateOrganizationUserDto(
            organizationId, request.UserId, OrganizationUserRole.Admin);
        await _organizationUserService.CreateOrganizationUser(createOrganizationUserDto, cancellationToken);
        
        return organizationId;
    }
}