namespace NightTasker.UserHub.Core.Domain.Entities;

/// <summary>
/// Связь пользователя с организацией.
/// </summary>
public class OrganizationUser
{
    /// <summary>
    /// ИД пользователя.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Пользователь.
    /// </summary>
    public UserInfo UserInfo { get; set; } = null!;

    /// <summary>
    /// ИД организации.
    /// </summary>
    public Guid OrganizationId { get; set; }
    
    /// <summary>
    /// Организация.
    /// </summary>
    public Organization Organization { get; set; } = null!;
}