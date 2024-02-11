using NightTasker.Common.Core.Abstractions;

namespace NightTasker.UserHub.Core.Domain.Entities;

public class UserImage : IEntityWithId<Guid>, IDateTimeOffsetModification
{
    private UserImage(
        Guid id,
        Guid userId,
        string? fileName,
        string? extension,
        string? contentType,
        long fileSize)
    {
        Id = id;
        FileName = fileName;
        Extension = extension;
        ContentType = contentType;
        FileSize = fileSize;
        UserId = userId;
    }

    public static UserImage CreateInstance(
        Guid id,
        Guid userId,
        string? fileName,
        string? extension,
        string? contentType,
        long fileSize)
    {
        return new UserImage(id, userId, fileName, extension, contentType, fileSize);
    }
    
    public static UserImage CreateInstance(
        Guid userId,
        string? fileName,
        string? extension,
        string? contentType,
        long fileSize)
    {
        return new UserImage(Guid.NewGuid(), userId, fileName, extension, contentType, fileSize);
    }

    public Guid Id { get; private set; }

    public bool IsActive { get; private set; }

    public string? FileName { get; private set; }

    public string? Extension { get; private set; }

    public string? ContentType { get; private set; }

    public long FileSize { get; private set; }
        
    public Guid UserId { get; private set; }

    public User? User { get; private set; }

    public DateTimeOffset CreatedDateTimeOffset { get; set; }
    
    public DateTimeOffset? UpdatedDateTimeOffset { get; set; }

    public void SetActive()
    {
        IsActive = true;
    }

    public void SetInactive()
    {
        IsActive = false;
    }
}