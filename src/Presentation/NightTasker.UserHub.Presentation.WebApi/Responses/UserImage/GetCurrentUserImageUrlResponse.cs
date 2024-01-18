namespace NightTasker.UserHub.Presentation.WebApi.Responses.UserImage;

/// <summary>
/// Ответ на запрос для получения ссылки на фотографию текущего пользователя.
/// </summary>
/// <param name="Url">Ссылка на фотографию текущего пользователя.</param>
public record GetCurrentUserImageUrlResponse(string Url);