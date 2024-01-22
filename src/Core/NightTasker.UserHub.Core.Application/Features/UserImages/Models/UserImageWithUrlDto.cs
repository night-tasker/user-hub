namespace NightTasker.UserHub.Core.Application.Features.UserImages.Models;

/// <summary>
/// Сведения о пользовательской фотографии с URL.
/// </summary>
/// <param name="Id">ИД.</param>
/// <param name="Url">URL.</param>
/// <param name="IsActive">Активная ли фотография.</param>
public record UserImageWithUrlDto(Guid Id, string Url, bool IsActive);