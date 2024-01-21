using NightTasker.Common.Core.Exceptions.Base;

namespace NightTasker.UserHub.Core.Application.Exceptions.NotFound;

/// <summary>
/// Исключение, возникающее при отсутствии файлов в хранилище.
/// </summary>
/// <param name="fileIds">Идентификаторы файлов.</param>
public class StorageFilesNotFoundException(IReadOnlyCollection<Guid> fileIds)
    : NotFoundException($"Files in storage not found: {string.Join(", ", fileIds)}");