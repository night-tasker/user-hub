using MediatR;
using NightTasker.UserHub.Core.Domain.Primitives;

namespace NightTasker.UserHub.Core.Domain.Events.Organization;

public sealed record OrganizationCreatedDomainEvent(Guid OrganizationId) : IDomainEvent, INotification;