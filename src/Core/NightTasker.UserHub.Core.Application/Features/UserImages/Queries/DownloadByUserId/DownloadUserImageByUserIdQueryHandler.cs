using MediatR;
using NightTasker.UserHub.Core.Application.Features.UserImages.Models;
using NightTasker.UserHub.Core.Application.Features.UserImages.Services.Contracts;

namespace NightTasker.UserHub.Core.Application.Features.UserImages.Queries.DownloadByUserId;

internal class DownloadUserImageByUserIdQueryHandler(
    IUserImageService userImageService) : IRequestHandler<DownloadUserImageByUserIdQuery, UserImageWithStreamDto>
{
    private readonly IUserImageService _userImageService = userImageService ?? throw new ArgumentNullException(nameof(userImageService));
    
    public async Task<UserImageWithStreamDto> Handle(DownloadUserImageByUserIdQuery request, CancellationToken cancellationToken)
    {
        var userImageDto = await _userImageService.DownloadActiveUserImageByUserId(request.UserId, cancellationToken);
        return userImageDto;
    }
}