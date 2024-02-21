using MassTransit;
using MediatR;
using NightTasker.Common.Messaging.DataTransferObjects.Organizations;
using NightTasker.Common.Messaging.Events.Implementations.Organizations;
using NightTasker.UserHub.Core.Application.Exceptions.NotFound;
using NightTasker.UserHub.Core.Domain.Events.Organization;
using NightTasker.UserHub.Core.Domain.Repositories;
using OrganizationUserRole = NightTasker.Common.Messaging.Enums.OrganizationUserRole;

namespace NightTasker.UserHub.Core.Application.Features.Organizations.Events;

internal sealed class OrganizationCreatedDomainEventHandler(
    IUnitOfWork unitOfWork,
    IPublishEndpoint publishEndpoint)
    : INotificationHandler<OrganizationCreatedDomainEvent>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    private readonly IPublishEndpoint _publishEndpoint = 
        publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
    
    public async Task Handle(OrganizationCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var organization = await _unitOfWork.OrganizationRepository.TryGetOrganizationByIdWithUser(
            notification.OrganizationId, false, cancellationToken);
        if (organization is null)
        {
            throw new OrganizationNotFoundException(notification.OrganizationId);
        }

        var createdUsers = organization.OrganizationUsers
            .Select(x => new OrganizationUserDto(x.UserId, x.OrganizationId, (OrganizationUserRole)x.Role))
            .ToArray();
        var organizationCreated = new OrganizationCreated(organization.Id, createdUsers);
        await _publishEndpoint.Publish(organizationCreated, cancellationToken);
    }
}