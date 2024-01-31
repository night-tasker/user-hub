using Google.Protobuf;
using Grpc.Core;
using Microsoft.Extensions.Options;
using NightTasker.Common.Grpc.StorageFiles;
using NightTasker.UserHub.Core.Application.ApplicationContracts.Services;
using NightTasker.UserHub.Core.Application.Exceptions.NotFound;
using NightTasker.UserHub.Core.Application.Models.StorageFile;
using NightTasker.UserHub.Infrastructure.Grpc.Settings;

namespace NightTasker.UserHub.Infrastructure.Grpc.Implementations.Client.StorageFile;

public class StorageFileService(
    Common.Grpc.StorageFiles.StorageFile.StorageFileClient storageFileClient,
    IOptions<StorageGrpcSettings> storageGrpcSettings) : IStorageFileService
{
    private readonly Common.Grpc.StorageFiles.StorageFile.StorageFileClient _storageFileClient =
        storageFileClient ?? throw new ArgumentNullException(nameof(storageFileClient));
    private readonly StorageGrpcSettings _storageGrpcSettings =
        storageGrpcSettings.Value ?? throw new ArgumentNullException(nameof(storageGrpcSettings));
    
    public async Task<DownloadedFileDto> DownloadFile(
        Guid fileId, CancellationToken cancellationToken)
    {
        var downloadFileRequest = new DownloadFileRequest
        {
            BucketName = _storageGrpcSettings.BucketName,
            FileName = fileId.ToString()
        };
        var callOptions = new CallOptions(cancellationToken: cancellationToken);
        var response = await _storageFileClient.DownloadFileAsync(downloadFileRequest, callOptions);
        var downloadedFileDto = new DownloadedFileDto(response.Data.ToByteArray());
        return downloadedFileDto;
    }

    public async Task UploadFile(UploadFileDto uploadFileDto, CancellationToken cancellationToken)
    {
        var uploadFileRequest = new UploadFileRequest
        {
            BucketName = _storageGrpcSettings.BucketName,
            FileName = uploadFileDto.FileName,
            Data = ByteString.CopyFrom(uploadFileDto.Stream.ToArray()),
            ContentType = uploadFileDto.ContentType,
            FileSize = uploadFileDto.Length
        };
        await _storageFileClient.UploadFileAsync(uploadFileRequest, cancellationToken: cancellationToken);
    }

    public async Task<string> GetFileUrl(Guid fileId, CancellationToken cancellationToken)
    {
        var getFileUrlRequest = new GetFileUrlRequest
        {
            BucketName = _storageGrpcSettings.BucketName,
            FileName = fileId.ToString()
        };
        
        var callOptions = new CallOptions(cancellationToken: cancellationToken);
        var response = await _storageFileClient.GetFileUrlAsync(getFileUrlRequest, callOptions);
        return response.Url;
    }

    public async Task<IDictionary<Guid, string>> GetFilesUrls(
        HashSet<Guid> fileIds, CancellationToken cancellationToken)
    {
        var request = new GetFilesUrlRequest();
        var requestFiles = fileIds
            .Select(fileId => new GetFileUrlRequest
            {
                BucketName = _storageGrpcSettings.BucketName,
                FileName = fileId.ToString()
            })
            .ToList();
        request.Files.AddRange(requestFiles);
        
        var callOptions = new CallOptions(cancellationToken: cancellationToken);
        var response = await _storageFileClient.GetFilesUrlAsync(request, callOptions);
        
        var dictionary = response.Files.ToDictionary(file => Guid.Parse(file.FileName), file => file.Url);
        ValidateFilesCount(fileIds, dictionary.Keys.ToHashSet());

        return dictionary;
    }

    public async Task RemoveFile(Guid fileId, CancellationToken cancellationToken)
    {
        var removeFileRequest = new RemoveFileRequest
        {
            BucketName = _storageGrpcSettings.BucketName,
            FileName = fileId.ToString()
        };
        var callOptions = new CallOptions(cancellationToken: cancellationToken);
        await _storageFileClient.RemoveFileAsync(removeFileRequest, callOptions);
    }

    private static void ValidateFilesCount(
        IReadOnlyCollection<Guid> lookingForFileIds, IReadOnlyCollection<Guid> foundFileIds)
    {
        if (lookingForFileIds.Count == foundFileIds.Count)
        {
            return;
        }
        var notFoundFileIds = lookingForFileIds.Except(foundFileIds).ToList();
        throw new StorageFilesNotFoundException(notFoundFileIds);
    }
}