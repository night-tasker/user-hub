namespace NightTasker.UserHub.Core.Application.Features.UserImages.Models;

public record CreateUserImageDto(
    Guid UserInfoId,
    string FileName,
    string Extension,
    string ContentType,
    long FileSize)
{
    public Domain.Entities.UserImage ToEntity()
    {
        return new Domain.Entities.UserImage
        {
            UserInfoId = UserInfoId,
            FileName = FileName,
            Extension = Extension,
            ContentType = ContentType,
            FileSize = FileSize
        };
    }
};