namespace NightTasker.UserHub.Core.Application.Models.StorageFile;

/// <summary>
/// DTO для загрузки файла.
/// </summary>
/// <param name="FileName">Имя файла.</param>
/// <param name="Stream">Поток.</param>
/// <param name="ContentType">Тип содержимого.</param>
/// <param name="Length">Длина.</param>
public record UploadFileDto(string FileName, MemoryStream Stream, string ContentType, long Length);