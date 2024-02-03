using NightTasker.Common.Core.Exceptions.Base;

namespace NightTasker.UserHub.Core.Application.Exceptions.NotFound;

public class ActiveUserImageForUserIdNotFoundException(Guid userId) 
    : NotFoundException($"Active user image for user with id {userId} not found.");