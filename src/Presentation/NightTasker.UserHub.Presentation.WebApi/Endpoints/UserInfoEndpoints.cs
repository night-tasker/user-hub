namespace NightTasker.UserHub.Presentation.WebApi.Endpoints;

/// <summary>
/// Пути эндпоинтов для работы со сведениями пользователей.
/// </summary>
public static class UserInfoEndpoints
{
    /// <summary>
    /// Путь для получения информации о пользователе (основной).
    /// </summary>
    public const string UserInfoResource = "user-info";
    
    /// <summary>
    /// Путь для получения информации о текущем пользователе.
    /// </summary>
    public const string CurrentUserInfo = "current";
    
    /// <summary>
    /// Путь для обновления <see cref="UserInfo"/>.
    /// </summary>
    public const string UpdateUserInfo = "current";
}