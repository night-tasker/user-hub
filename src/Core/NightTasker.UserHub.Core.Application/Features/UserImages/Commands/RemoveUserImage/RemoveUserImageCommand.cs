using MediatR;

namespace NightTasker.UserHub.Core.Application.Features.UserImages.Commands.RemoveUserImage;

public record RemoveUserImageCommand(Guid UserId, Guid UserImageId) : IRequest;