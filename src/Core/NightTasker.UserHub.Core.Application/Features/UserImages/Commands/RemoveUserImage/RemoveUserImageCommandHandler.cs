using MediatR;
using NightTasker.UserHub.Core.Application.Features.UserImages.Services.Contracts;

namespace NightTasker.UserHub.Core.Application.Features.UserImages.Commands.RemoveUserImage;

internal class RemoveUserImageCommandHandler(IUserImageService userImageService) : IRequestHandler<RemoveUserImageCommand>
{
    private readonly IUserImageService _userImageService = 
        userImageService ?? throw new ArgumentNullException(nameof(userImageService));

    public Task Handle(RemoveUserImageCommand request, CancellationToken cancellationToken)
    {
        return _userImageService.RemoveUserImageById(request.UserId, request.UserImageId, cancellationToken);
    }
}