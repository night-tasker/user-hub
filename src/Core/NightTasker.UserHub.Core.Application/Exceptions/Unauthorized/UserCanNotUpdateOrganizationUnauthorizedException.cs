using NightTasker.Common.Core.Exceptions.Base;

namespace NightTasker.UserHub.Core.Application.Exceptions.Unauthorized;

public class UserCanNotUpdateOrganizationUnauthorizedException(Guid organizationId, Guid userId)
    : UnauthorizedException($"User with id {userId} can not update organization with id {organizationId}");