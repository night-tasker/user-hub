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
        return Domain.Entities.UserImage.CreateInstance(
            UserId, FileName, Extension, ContentType, FileSize);
    }
}