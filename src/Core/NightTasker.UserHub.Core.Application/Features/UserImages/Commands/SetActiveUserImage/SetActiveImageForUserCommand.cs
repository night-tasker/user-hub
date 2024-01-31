using MediatR;

namespace NightTasker.UserHub.Core.Application.Features.UserImages.Commands.SetActiveUserImage;

public record SetActiveImageForUserCommand(Guid UserInfoId, Guid UserImageId) : IRequest;