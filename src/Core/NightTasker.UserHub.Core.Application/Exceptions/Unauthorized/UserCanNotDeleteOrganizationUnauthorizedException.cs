using NightTasker.Common.Core.Exceptions.Base;

namespace NightTasker.UserHub.Core.Application.Exceptions.Unauthorized;

public class UserCanNotDeleteOrganizationUnauthorizedException(Guid organizationId, Guid userId)
    : UnauthorizedException($"User with id {userId} can not delete organization with id {organizationId}");