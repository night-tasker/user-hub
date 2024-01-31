using NightTasker.Common.Core.Exceptions.Base;

namespace NightTasker.UserHub.Core.Application.Exceptions.NotFound;

public class UserImageWithIdNotFoundException(Guid userImageId)
    : NotFoundException($"User image with id {userImageId} not found");