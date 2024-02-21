using MediatR;
using NightTasker.UserHub.Core.Domain.Primitives;

namespace NightTasker.UserHub.Core.Domain.Events.OrganizationUsers;

public sealed record OrganizationUserCreatedDomainEvent(
    Guid OrganizationId, Guid UserId) : IDomainEvent, INotification;