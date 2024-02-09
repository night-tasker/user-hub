using MediatR;
using NightTasker.UserHub.Core.Application.Features.Organizations.Services.Contracts;
using NightTasker.UserHub.Core.Domain.Repositories;

namespace NightTasker.UserHub.Core.Application.Features.Organizations.Commands.RemoveOrganizationAsUser;

public class RemoveOrganizationAsUserCommandHandler(IOrganizationService organizationService, IUnitOfWork unitOfWork)
    : IRequestHandler<RemoveOrganizationAsUserCommand>
{
    private readonly IOrganizationService _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

    public async Task Handle(RemoveOrganizationAsUserCommand request, CancellationToken cancellationToken)
    {
        await _organizationService.RemoveOrganizationAsUser(request.UserId, request.OrganizationId, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);
    }
}