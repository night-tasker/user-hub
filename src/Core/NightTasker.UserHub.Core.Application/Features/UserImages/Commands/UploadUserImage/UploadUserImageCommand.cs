using MediatR;

namespace NightTasker.UserHub.Core.Application.Features.UserImages.Commands.UploadUserImage;

/// <summary>
/// Загрузка фото пользователя.
/// </summary>
/// <param name="UserId">ИД пользователя.</param>
/// <param name="Stream">Поток.</param>
/// <param name="FileName">Имя файла.</param>
/// <param name="FileExtension">Расширение файла.</param>
/// <param name="ContentType">Тип контента.</param>
/// <param name="FileSize">Размер файла.</param>
public record UploadUserImageCommand(
    Guid UserId,
    Stream Stream, 
    string FileName,
    string FileExtension,
    string ContentType, 
    long FileSize) : IRequest;