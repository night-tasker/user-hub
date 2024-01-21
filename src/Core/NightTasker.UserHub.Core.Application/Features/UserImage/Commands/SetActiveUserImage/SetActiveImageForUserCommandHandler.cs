using MediatR;
using NightTasker.UserHub.Core.Application.Features.UserImage.Services.Contracts;

namespace NightTasker.UserHub.Core.Application.Features.UserImage.Commands.SetActiveUserImage;

/// <summary>
/// Хэндлер для <see cref="SetActiveImageForUserCommand"/>.
/// </summary>
public class SetActiveImageForUserCommandHandler(IUserImageService userImageService)
    : IRequestHandler<SetActiveImageForUserCommand>
{
    private readonly IUserImageService _userImageService = 
        userImageService ?? throw new ArgumentNullException(nameof(userImageService));

    public Task Handle(SetActiveImageForUserCommand request, CancellationToken cancellationToken)
    {
        return _userImageService.SetActiveUserImageForUserInfoId(
            request.UserInfoId, request.UserImageId, cancellationToken);
    }
}