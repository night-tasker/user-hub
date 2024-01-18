using NightTasker.Common.Core.Abstractions;

namespace NightTasker.UserHub.Core.Domain.Entities;

/// <summary>
/// Фотография пользователя.
/// </summary>
public class UserImage : IEntityWithId<Guid>, IDateTimeOffsetModification
{
    /// <summary>
    /// ИД фотографии.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Активна ли фотография (активна может быть только одна).
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Название файла.
    /// </summary>
    public string? FileName { get; set; }
    
    /// <summary>
    /// Расширение файла.
    /// </summary>
    public string? Extension { get; set; }
    
    /// <summary>
    /// Тип содержимого.
    /// </summary>
    public string? ContentType { get; set; }
    
    /// <summary>
    /// Размер файла.
    /// </summary>
    public long FileSize { get; set; }
        
    /// <summary>
    /// ИД пользователя.
    /// </summary>
    public Guid UserInfoId { get; set; }

    /// <summary>
    /// Сведения о пользователе.
    /// </summary>
    public UserInfo? UserInfo { get; set; }

    /// <inheritdoc />
    public DateTimeOffset CreatedDateTimeOffset { get; set; }
    
    /// <inheritdoc />
    public DateTimeOffset? UpdatedDateTimeOffset { get; set; }
}