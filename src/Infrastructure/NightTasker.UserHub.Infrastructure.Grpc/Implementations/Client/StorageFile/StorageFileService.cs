using Google.Protobuf;
using Grpc.Core;
using Microsoft.Extensions.Options;
using NightTasker.Common.Grpc.StorageFiles;
using NightTasker.UserHub.Core.Application.ApplicationContracts.Services;
using NightTasker.UserHub.Core.Application.Models.StorageFile;
using NightTasker.UserHub.Infrastructure.Grpc.Settings;

namespace NightTasker.UserHub.Infrastructure.Grpc.Implementations.Client.StorageFile;

/// <inheritdoc />
public class StorageFileService(
    Common.Grpc.StorageFiles.StorageFile.StorageFileClient storageFileClient,
    IOptions<StorageGrpcSettings> storageGrpcSettings) : IStorageFileService
{
    private readonly Common.Grpc.StorageFiles.StorageFile.StorageFileClient _storageFileClient =
        storageFileClient ?? throw new ArgumentNullException(nameof(storageFileClient));
    private readonly StorageGrpcSettings _storageGrpcSettings =
        storageGrpcSettings?.Value ?? throw new ArgumentNullException(nameof(storageGrpcSettings));
    
    /// <inheritdoc />
    public async Task<DownloadedFileDto> DownloadFile(
        Guid fileId, CancellationToken cancellationToken)
    {
        var downloadFileRequest = new DownloadFileRequest()
        {
            BucketName = _storageGrpcSettings.BucketName,
            FileName = fileId.ToString()
        };
        var callOptions = new CallOptions(cancellationToken: cancellationToken);
        var response = await _storageFileClient.DownloadFileAsync(downloadFileRequest, callOptions);
        var downloadedFileDto = new DownloadedFileDto(response.Data.ToByteArray());
        return downloadedFileDto;
    }

    /// <inheritdoc />
    public async Task UploadFile(UploadFileDto uploadFileDto, CancellationToken cancellationToken)
    {
        var uploadFileRequest = new UploadFileRequest()
        {
            BucketName = _storageGrpcSettings.BucketName,
            FileName = uploadFileDto.FileName,
            Data = ByteString.CopyFrom(uploadFileDto.Stream.ToArray()),
            ContentType = uploadFileDto.ContentType,
            FileSize = uploadFileDto.Length
        };
        await _storageFileClient.UploadFileAsync(uploadFileRequest, cancellationToken: cancellationToken);
    }
}