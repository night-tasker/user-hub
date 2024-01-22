using MediatR;
using NightTasker.UserHub.Core.Application.Features.UserImages.Models;
using NightTasker.UserHub.Core.Application.Features.UserImages.Services.Contracts;

namespace NightTasker.UserHub.Core.Application.Features.UserImages.Queries.DownloadByUserId;

/// <summary>
/// Хэндлер для <see cref="DownloadUserImageByUserInfoIdQuery"/>
/// </summary>
internal class DownloadUserImageByUserInfoIdQueryHandler(
    IUserImageService userImageService) : IRequestHandler<DownloadUserImageByUserInfoIdQuery, UserImageWithStreamDto>
{
    private readonly IUserImageService _userImageService = userImageService ?? throw new ArgumentNullException(nameof(userImageService));
    
    public async Task<UserImageWithStreamDto> Handle(DownloadUserImageByUserInfoIdQuery request, CancellationToken cancellationToken)
    {
        var userImageDto = await _userImageService.DownloadUserImageByUserInfoId(request.UserInfoId, cancellationToken);
        return userImageDto;
    }
}