using MediatR;
using NightTasker.UserHub.Core.Domain.Events.Organization;
using NightTasker.UserHub.Core.Domain.Repositories;

namespace NightTasker.UserHub.Core.Application.Features.Organizations.Events;

public class OrganizationCreatedDomainEventHandler(IUnitOfWork unitOfWork)
    : INotificationHandler<OrganizationCreatedDomainEvent>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

    public async Task Handle(OrganizationCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var organization = await _unitOfWork.OrganizationRepository.TryGetOrganizationById(
            notification.OrganizationId, false, cancellationToken);
        return;
    }
}