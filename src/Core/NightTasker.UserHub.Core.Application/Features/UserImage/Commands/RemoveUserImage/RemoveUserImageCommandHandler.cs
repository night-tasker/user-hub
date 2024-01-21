using MediatR;
using NightTasker.UserHub.Core.Application.Features.UserImage.Services.Contracts;

namespace NightTasker.UserHub.Core.Application.Features.UserImage.Commands.RemoveUserImage;

/// <summary>
/// Хэндлер <see cref="RemoveUserImageCommand"/>.
/// </summary>
public class RemoveUserImageCommandHandler(IUserImageService userImageService) : IRequestHandler<RemoveUserImageCommand>
{
    private readonly IUserImageService _userImageService = 
        userImageService ?? throw new ArgumentNullException(nameof(userImageService));

    public Task Handle(RemoveUserImageCommand request, CancellationToken cancellationToken)
    {
        return _userImageService.RemoveUserImageById(request.UserImageId, cancellationToken);
    }
}