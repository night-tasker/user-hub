namespace NightTasker.UserHub.Core.Application.Features.UserImages.Models;

/// <summary>
/// Сведения о пользовательской фотографии.
/// </summary>
/// <param name="Stream">Поток.</param>
/// <param name="FileName">Название файла.</param>
/// <param name="ContentType">Тип содержимого.</param>
public record UserImageWithStreamDto(Stream Stream, string FileName, string Extension, string ContentType);