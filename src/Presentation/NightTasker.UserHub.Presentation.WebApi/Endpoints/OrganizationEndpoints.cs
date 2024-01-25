namespace NightTasker.UserHub.Presentation.WebApi.Endpoints;

/// <summary>
/// Пути эндпоинтов для работы с организациями.
/// </summary>
public static class OrganizationEndpoints
{
    /// <summary>
    /// Базовый путь для получения организации по идентификатору.
    /// </summary>
    public const string BaseResource = "organizations";
    
    /// <summary>
    /// Путь для получения организации по идентификатору.
    /// </summary>
    public const string GetById = "{organizationId}";
    
    /// <summary>
    /// Путь для получения роли пользователя в организации.
    /// </summary>
    public const string GetUserOrganizationRole = "{organizationId}/role";
}