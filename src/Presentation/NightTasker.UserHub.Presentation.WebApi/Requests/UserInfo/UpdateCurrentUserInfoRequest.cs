namespace NightTasker.UserHub.Presentation.WebApi.Requests.UserInfo;

/// <summary>
/// Запрос на обновление <see cref="UserInfo"/> текущего пользователя.
/// </summary>
/// <param name="FirstName">Имя.</param>
/// <param name="MiddleName">Отчество.</param>
/// <param name="LastName">Фамилия.</param>
public record UpdateCurrentUserInfoRequest(string FirstName, string MiddleName, string LastName);