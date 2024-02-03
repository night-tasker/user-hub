using MediatR;

namespace NightTasker.UserHub.Core.Application.Features.UserImages.Commands.SetActiveUserImage;

public record SetActiveImageForUserCommand(Guid UserId, Guid UserImageId) : IRequest;