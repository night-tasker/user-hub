using MediatR;

namespace NightTasker.UserHub.Core.Application.Features.UserImages.Commands.RemoveUserImage;

/// <summary>
/// Команда для удаления <see cref="UserImages"/>.
/// </summary>
/// <param name="UserImageId">ИД <see cref="UserImages"/>.</param>
public record RemoveUserImageCommand(Guid UserImageId) : IRequest;