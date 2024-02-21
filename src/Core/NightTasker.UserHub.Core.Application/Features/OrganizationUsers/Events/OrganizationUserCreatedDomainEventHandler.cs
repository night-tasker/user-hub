using MassTransit;
using MediatR;
using NightTasker.Common.Messaging.Enums;
using NightTasker.Common.Messaging.Events.Implementations.OrganizationUsers;
using NightTasker.UserHub.Core.Application.Exceptions.NotFound;
using NightTasker.UserHub.Core.Domain.Events.OrganizationUsers;
using NightTasker.UserHub.Core.Domain.Repositories;

namespace NightTasker.UserHub.Core.Application.Features.OrganizationUsers.Events;

internal sealed class OrganizationUserCreatedDomainEventHandler(
    IUnitOfWork unitOfWork,
    IPublishEndpoint publishEndpoint) : INotificationHandler<OrganizationUserCreatedDomainEvent>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    private readonly IPublishEndpoint _publishEndpoint = 
        publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
    
    public async Task Handle(OrganizationUserCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var organizationUser = await _unitOfWork.OrganizationUserRepository.TryGetByOrganizationAndUserIds(
            notification.OrganizationId, notification.UserId, cancellationToken);
        if (organizationUser is null)
        {
            throw new OrganizationUserNotFoundException(notification.OrganizationId, notification.UserId);
        }
        
        var organizationUserCreated = new OrganizationUserCreated(
            organizationUser.UserId, organizationUser.OrganizationId, (OrganizationUserRole)organizationUser.Role);
        await _publishEndpoint.Publish(organizationUserCreated, cancellationToken);
    }
}