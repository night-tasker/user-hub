namespace NightTasker.UserHub.Presentation.WebApi.Endpoints;

/// <summary>
/// Пути эндпоинтов для работы со фото пользователей.
/// </summary>
public class UserImageEndpoints
{
    /// <summary>
    /// Путь для получения фотографии пользователя (основной).
    /// </summary>
    public const string UserImageResource = "user-images";
    
    /// <summary>
    /// Путь для получения фотографии текущего пользователя.
    /// </summary>
    public const string CurrentUserImage = "current-user";
    
    /// <summary>
    /// Путь для загрузки фотографии текущего пользователя.
    /// </summary>
    public const string UploadCurrentUserImage = "upload-current-user-image";
}