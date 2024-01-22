using NightTasker.UserHub.Core.Application.Features.UserImages.Models;

namespace NightTasker.UserHub.Presentation.WebApi.Responses.UserImage;

/// <summary>
/// Ответ для получения ссылок на фотографии пользователя.
/// </summary>
/// <param name="Images">Ссылки на фотографии пользователя.</param>
public record GetCurrentUserImagesUrlResponse(IReadOnlyCollection<UserImageWithUrlDto> Images);