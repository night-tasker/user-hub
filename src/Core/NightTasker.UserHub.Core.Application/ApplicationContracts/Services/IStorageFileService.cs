using NightTasker.UserHub.Core.Application.Models.StorageFile;

namespace NightTasker.UserHub.Core.Application.ApplicationContracts.Services;

/// <summary>
/// Сервис для работы с файлами в хранилище.
/// </summary>
public interface IStorageFileService
{
    /// <summary>
    /// Скачать файл.
    /// </summary>
    /// <param name="fileId">ИД файла.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Скачиваемый файл.</returns>
    Task<DownloadedFileDto> DownloadFile(
        Guid fileId, CancellationToken cancellationToken);

    /// <summary>
    /// Загрузить файл.
    /// </summary>
    /// <param name="uploadFileDto">DTO загружаемого файла.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    Task UploadFile(UploadFileDto uploadFileDto, CancellationToken cancellationToken);
    
    /// <summary>
    /// Получить ссылку на файл.
    /// </summary>
    /// <param name="fileId">ИД файла.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Ссылка на файл.</returns>
    Task<string> GetFileUrl(Guid fileId, CancellationToken cancellationToken);
}