using MediatR;

namespace NightTasker.UserHub.Core.Application.Features.UserImages.Commands.UploadUserImage;

public record UploadUserImageCommand(
    Guid UserId,
    Stream Stream, 
    string FileName,
    string FileExtension,
    string ContentType, 
    long FileSize) : IRequest;