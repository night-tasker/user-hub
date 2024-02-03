using MediatR;
using NightTasker.UserHub.Core.Application.Features.Organizations.Services.Contracts;
using NightTasker.UserHub.Core.Application.Features.OrganizationUsers.Models;
using NightTasker.UserHub.Core.Application.Features.OrganizationUsers.Services.Contracts;
using NightTasker.UserHub.Core.Domain.Enums;

namespace NightTasker.UserHub.Core.Application.Features.Organizations.Commands.CreateOrganizationAsUser;

internal class CreateOrganizationAsUserCommandHandler(
    IOrganizationService organizationService,
    IOrganizationUserService organizationUserService)
    : IRequestHandler<CreateOrganizationAsUserCommand, Guid>
{
    private readonly IOrganizationService _organizationService = 
        organizationService ?? throw new ArgumentNullException(nameof(organizationService));
    private readonly IOrganizationUserService _organizationUserService = 
        organizationUserService ?? throw new ArgumentNullException(nameof(organizationUserService));

    public async Task<Guid> Handle(CreateOrganizationAsUserCommand request, CancellationToken cancellationToken)
    {
        var createOrganizationDto = request.ToCreateOrganizationDto();
        var organizationId = await _organizationService.CreateOrganization(
            createOrganizationDto, cancellationToken);
        
        var createOrganizationUserDto = new CreateOrganizationUserDto(
            organizationId, request.UserId, OrganizationUserRole.Admin);
        await _organizationUserService.CreateOrganizationUser(createOrganizationUserDto, cancellationToken);
        
        return organizationId;
    }
}