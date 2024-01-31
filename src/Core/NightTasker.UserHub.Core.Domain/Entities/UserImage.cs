using NightTasker.Common.Core.Abstractions;

namespace NightTasker.UserHub.Core.Domain.Entities;

public class UserImage : IEntityWithId<Guid>, IDateTimeOffsetModification
{
    public Guid Id { get; set; }

    public bool IsActive { get; set; }

    public string? FileName { get; set; }

    public string? Extension { get; set; }

    public string? ContentType { get; set; }

    public long FileSize { get; set; }
        
    public Guid UserInfoId { get; set; }

    public UserInfo? UserInfo { get; set; }

    public DateTimeOffset CreatedDateTimeOffset { get; set; }
    
    public DateTimeOffset? UpdatedDateTimeOffset { get; set; }
}