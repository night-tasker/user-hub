using NightTasker.Common.Core.Exceptions.Base;

namespace NightTasker.UserHub.Core.Application.Exceptions.NotFound;

public class ActiveUserImageForUserInfoIdNotFoundException(Guid userInfoId) 
    : NotFoundException($"Active user image for user with id {userInfoId} not found.");