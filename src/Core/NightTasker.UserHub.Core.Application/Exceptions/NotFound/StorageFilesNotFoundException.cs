using NightTasker.Common.Core.Exceptions.Base;

namespace NightTasker.UserHub.Core.Application.Exceptions.NotFound;

public class StorageFilesNotFoundException(IEnumerable<Guid> fileIds)
    : NotFoundException($"Files in storage not found: {string.Join(", ", fileIds)}");