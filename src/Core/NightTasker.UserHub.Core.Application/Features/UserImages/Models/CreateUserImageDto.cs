namespace NightTasker.UserHub.Core.Application.Features.UserImages.Models;

public record CreateUserImageDto(
    Guid UserInfoId, 
    string FileName,
    string Extension,
    string ContentType, 
    long FileSize);