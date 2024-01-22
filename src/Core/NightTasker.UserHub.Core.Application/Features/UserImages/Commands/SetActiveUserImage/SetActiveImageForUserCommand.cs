using MediatR;

namespace NightTasker.UserHub.Core.Application.Features.UserImages.Commands.SetActiveUserImage;

/// <summary>
/// Установить активную фотографию для пользователя.
/// </summary>
/// <param name="UserInfoId">ИД пользователя.</param>
/// <param name="UserImageId">ИД фотографии.</param>
public record SetActiveImageForUserCommand(Guid UserInfoId, Guid UserImageId) : IRequest;