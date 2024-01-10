using NightTasker.Common.Core.Abstractions;

namespace NightTasker.UserHub.Core.Domain.Entities;

/// <summary>
/// Сведения о пользователе.
/// </summary>
public class UserInfo : IEntityWithId<Guid>, IDateTimeOffsetModification
{
    /// <summary>
    /// UserId в Identity.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Имя пользователя.
    /// </summary>
    public string? UserName { get; set; }

    /// <summary>
    /// Адрес электронной почты.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Имя.
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// Отчество.
    /// </summary>
    public string? MiddleName { get; set; }
    
    /// <summary>
    /// Фамилия.
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// ИД фотографии.
    /// </summary>
    public Guid UserInfoImageId { get; set; }

    /// <summary>
    /// Фотография пользователя.
    /// </summary>
    public UserInfoImage? UserInfoImage { get; set; }

    /// <inheritdoc />
    public DateTimeOffset CreatedDateTimeOffset { get; set; }
    
    /// <inheritdoc />
    public DateTimeOffset? UpdatedDateTimeOffset { get; set; }
}