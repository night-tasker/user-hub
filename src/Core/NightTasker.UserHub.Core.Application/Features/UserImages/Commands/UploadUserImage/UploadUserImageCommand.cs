using MediatR;
using NightTasker.UserHub.Core.Application.Features.UserImages.Models;

namespace NightTasker.UserHub.Core.Application.Features.UserImages.Commands.UploadUserImage;

public record UploadUserImageCommand(
    Guid UserId,
    Stream Stream,
    string FileName,
    string FileExtension,
    string ContentType,
    long FileSize) : IRequest
{
    public CreateUserImageDto ToCreateUserImageDto()
    {
        return new CreateUserImageDto(UserId, FileName, FileExtension, ContentType, FileSize);
    }
};