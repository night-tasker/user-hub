using MediatR;

namespace NightTasker.UserHub.Core.Application.Features.UserImage.Commands.RemoveUserImage;

/// <summary>
/// Команда для удаления <see cref="UserImage"/>.
/// </summary>
/// <param name="UserImageId">ИД <see cref="UserImage"/>.</param>
public record RemoveUserImageCommand(Guid UserImageId) : IRequest;