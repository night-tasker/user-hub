using MediatR;
using NightTasker.UserHub.Core.Application.Features.Organizations.Services.Contracts;
using NightTasker.UserHub.Core.Application.Features.OrganizationUsers.Models;
using NightTasker.UserHub.Core.Application.Features.OrganizationUsers.Services.Contracts;
using NightTasker.UserHub.Core.Domain.Enums;
using NightTasker.UserHub.Core.Domain.Repositories;

namespace NightTasker.UserHub.Core.Application.Features.Organizations.Commands.CreateOrganizationAsUser;

internal class CreateOrganizationAsUserCommandHandler(
    IOrganizationService organizationService,
    IOrganizationUserService organizationUserService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateOrganizationAsUserCommand, Guid>
{
    private readonly IOrganizationService _organizationService = 
        organizationService ?? throw new ArgumentNullException(nameof(organizationService));
    private readonly IOrganizationUserService _organizationUserService = 
        organizationUserService ?? throw new ArgumentNullException(nameof(organizationUserService));
    private readonly IUnitOfWork _unitOfWork = 
        unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

    public async Task<Guid> Handle(CreateOrganizationAsUserCommand request, CancellationToken cancellationToken)
    {
        var createOrganizationDto = request.ToCreateOrganizationDto();
        var organizationId = await _organizationService.CreateOrganizationAsUser(
            createOrganizationDto, request.UserId, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);
        return organizationId;
    }
}