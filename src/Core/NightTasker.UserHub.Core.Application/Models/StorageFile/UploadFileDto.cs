namespace NightTasker.UserHub.Core.Application.Models.StorageFile;

public record UploadFileDto(string FileName, MemoryStream Stream, string ContentType, long Length);