using MediatR;
using NightTasker.UserHub.Core.Application.Features.UserImages.Services.Contracts;

namespace NightTasker.UserHub.Core.Application.Features.UserImages.Commands.SetActiveUserImage;

internal class SetActiveImageForUserCommandHandler(IUserImageService userImageService)
    : IRequestHandler<SetActiveImageForUserCommand>
{
    private readonly IUserImageService _userImageService = 
        userImageService ?? throw new ArgumentNullException(nameof(userImageService));

    public Task Handle(SetActiveImageForUserCommand request, CancellationToken cancellationToken)
    {
        return _userImageService.SetActiveUserImageForUser(
            request.UserId, request.UserImageId, cancellationToken);
    }
}