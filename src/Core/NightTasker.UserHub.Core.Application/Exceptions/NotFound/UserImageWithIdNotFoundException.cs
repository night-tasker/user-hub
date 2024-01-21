using NightTasker.Common.Core.Exceptions.Base;
using NightTasker.UserHub.Core.Domain.Entities;

namespace NightTasker.UserHub.Core.Application.Exceptions.NotFound;

/// <summary>
/// Исключение для отсутствия <see cref="UserImage"/>.
/// </summary>
/// <param name="userImageId">ИД <see cref="UserImage"/>.</param>
public class UserImageWithIdNotFoundException(Guid userImageId)
    : NotFoundException($"User image with id {userImageId} not found");