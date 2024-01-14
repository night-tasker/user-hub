using NightTasker.Common.Core.Exceptions.Base;

namespace NightTasker.UserHub.Core.Application.Exceptions.NotFound;

/// <summary>
/// Исключение: изображение пользователя не найдено.
/// </summary>
/// <param name="userInfoId">ИД пользователя.</param>
public class UserImageForUserInfoIdNotFoundException(Guid userInfoId) 
    : NotFoundException($"User image for user with id {userInfoId} not found.");