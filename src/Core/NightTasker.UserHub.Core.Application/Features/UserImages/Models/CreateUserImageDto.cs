namespace NightTasker.UserHub.Core.Application.Features.UserImages.Models;

public record CreateUserImageDto(
    Guid UserId,
    string FileName,
    string Extension,
    string ContentType,
    long FileSize)
{
    public Domain.Entities.UserImage ToEntity()
    {
        return new Domain.Entities.UserImage
        {
            UserId = UserId,
            FileName = FileName,
            Extension = Extension,
            ContentType = ContentType,
            FileSize = FileSize
        };
    }
};