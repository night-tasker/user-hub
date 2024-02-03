using MediatR;
using NightTasker.UserHub.Core.Application.Features.Organizations.Services.Contracts;

namespace NightTasker.UserHub.Core.Application.Features.Organizations.Commands.UpdateOrganizationAsUser;

public class UpdateOrganizationAsUserCommandHandler(IOrganizationService organizationService)
    : IRequestHandler<UpdateOrganizationAsUserCommand>
{
    private readonly IOrganizationService _organizationService = 
        organizationService ?? throw new ArgumentNullException(nameof(organizationService));

    public Task Handle(UpdateOrganizationAsUserCommand request, CancellationToken cancellationToken)
    {
        return _organizationService.UpdateOrganizationAsUser(
            request.UserId, request.OrganizationId, request.UpdateOrganizationDto, cancellationToken);
    }
}