using NightTasker.UserHub.Core.Application.Models.StorageFile;

namespace NightTasker.UserHub.Core.Application.ApplicationContracts.Services;

public interface IStorageFileService
{
    Task<DownloadedFileDto> DownloadFile(
        Guid fileId, CancellationToken cancellationToken);

    Task UploadFile(UploadFileDto uploadFileDto, CancellationToken cancellationToken);
    
    Task<string> GetFileUrl(Guid fileId, CancellationToken cancellationToken);

    Task<IDictionary<Guid, string>> GetFilesUrls(
        HashSet<Guid> fileIds, CancellationToken cancellationToken);

    Task RemoveFile(Guid fileId, CancellationToken cancellationToken);
}