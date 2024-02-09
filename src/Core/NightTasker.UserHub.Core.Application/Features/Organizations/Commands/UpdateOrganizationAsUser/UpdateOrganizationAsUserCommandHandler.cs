using MediatR;
using NightTasker.UserHub.Core.Application.Features.Organizations.Services.Contracts;
using NightTasker.UserHub.Core.Domain.Repositories;

namespace NightTasker.UserHub.Core.Application.Features.Organizations.Commands.UpdateOrganizationAsUser;

public class UpdateOrganizationAsUserCommandHandler(IOrganizationService organizationService, IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateOrganizationAsUserCommand>
{
    private readonly IOrganizationService _organizationService = 
        organizationService ?? throw new ArgumentNullException(nameof(organizationService));

    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    public async Task Handle(UpdateOrganizationAsUserCommand request, CancellationToken cancellationToken)
    {
        await _organizationService.UpdateOrganizationAsUser(
            request.UserId, request.OrganizationId, request.UpdateOrganizationDto, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);
    }
}