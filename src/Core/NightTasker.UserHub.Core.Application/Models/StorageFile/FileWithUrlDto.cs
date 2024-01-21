namespace NightTasker.UserHub.Core.Application.Models.StorageFile;

/// <summary>
/// Файл с URL-ссылкой.
/// </summary>
/// <param name="FileName">Имя файла.</param>
/// <param name="Url">URL-ссылка.</param>
public record FileWithUrlDto(string FileName, string Url);