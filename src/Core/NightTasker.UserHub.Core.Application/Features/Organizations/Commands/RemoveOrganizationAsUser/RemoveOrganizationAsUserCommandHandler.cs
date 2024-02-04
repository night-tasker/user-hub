using MediatR;
using NightTasker.UserHub.Core.Application.Features.Organizations.Services.Contracts;

namespace NightTasker.UserHub.Core.Application.Features.Organizations.Commands.RemoveOrganizationAsUser;

public class RemoveOrganizationAsUserCommandHandler(IOrganizationService organizationService)
    : IRequestHandler<RemoveOrganizationAsUserCommand>
{
    private readonly IOrganizationService _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));

    public Task Handle(RemoveOrganizationAsUserCommand request, CancellationToken cancellationToken)
    {
        return _organizationService.RemoveOrganizationAsUser(request.UserId, request.OrganizationId, cancellationToken);
    }
}