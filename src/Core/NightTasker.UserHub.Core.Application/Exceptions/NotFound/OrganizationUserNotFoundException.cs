using NightTasker.Common.Core.Exceptions.Base;

namespace NightTasker.UserHub.Core.Application.Exceptions.NotFound;

public class OrganizationUserNotFoundException(Guid organizationId, Guid userId) 
    : NotFoundException($"User with id: {userId} not found in organization with id: {organizationId}");