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
    public const string UploadCurrentUserImage = "current-user/upload";

    /// <summary>
    /// Путь для получения ссылки на активную фотографию пользователя.
    /// </summary>
    public const string GetCurrentUserActiveImageUrl = "current-user/active/url";
    
    /// <summary>
    /// Путь для получения ссылок на фотографии пользователя.
    /// </summary>
    public const string GetCurrentUserImagesUrl = "current-user/url";
    
    /// <summary>
    /// Путь для установки активной фотографии пользователя.
    /// </summary>
    public const string SetActiveUserImage = "current-user/active/{userImageId}";
    
    /// <summary>
    /// Путь для удаления фотографии пользователя по идентификатору.
    /// </summary>
    public const string RemoveCurrentUserImageById = "current-user/{userImageId}";
}