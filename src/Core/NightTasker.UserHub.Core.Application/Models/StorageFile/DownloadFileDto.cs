namespace NightTasker.UserHub.Core.Application.Models.StorageFile;

/// <summary>
/// DTO для скачивания файла.
/// </summary>
/// <param name="FileId">Идентификатор файла.</param>
public record DownloadFileDto(Guid FileId);