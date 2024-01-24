namespace NightTasker.UserHub.Presentation.WebApi.Requests.Organization;

/// <summary>
/// Запрос на создание организации.
/// </summary>
/// <param name="Name">Имя организации.</param>
public record CreateOrganizationRequest(string Name, string? Description);