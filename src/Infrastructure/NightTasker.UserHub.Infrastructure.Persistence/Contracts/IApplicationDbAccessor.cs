using NightTasker.Common.Core.Persistence;
using NightTasker.UserHub.Core.Domain.Entities;

namespace NightTasker.UserHub.Infrastructure.Persistence.Contracts;

/// <summary>
/// Акссесор к базе данных.
/// </summary>
public interface IApplicationDbAccessor
{
    /// <summary>
    /// Сведения о пользователях.
    /// </summary>
    ApplicationDbSet<UserInfo, Guid> UserInfos { get; }
    
    /// <summary>
    /// Фотографии пользователей.
    /// </summary>
    ApplicationDbSet<UserImage, Guid> UserImages { get; }
    
    /// <summary>
    /// Организации.
    /// </summary>
    ApplicationDbSet<Organization, Guid> Organizations { get; }
    
    /// <summary>
    /// Сохранить изменения.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены.</param>
    Task SaveChanges(CancellationToken cancellationToken);
}