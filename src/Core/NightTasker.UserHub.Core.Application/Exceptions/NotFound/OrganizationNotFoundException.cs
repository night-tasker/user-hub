using NightTasker.Common.Core.Exceptions.Base;

namespace NightTasker.UserHub.Core.Application.Exceptions.NotFound;

public class OrganizationNotFoundException(Guid organizationId)
    : NotFoundException($"Organization with id: {organizationId} not found");