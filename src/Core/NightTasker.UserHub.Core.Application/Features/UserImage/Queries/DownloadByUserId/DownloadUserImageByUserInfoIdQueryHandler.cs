using MediatR;
using NightTasker.UserHub.Core.Application.Features.UserImage.Models;
using NightTasker.UserHub.Core.Application.Features.UserImage.Services.Contracts;

namespace NightTasker.UserHub.Core.Application.Features.UserImage.Queries.DownloadByUserId;

/// <summary>
/// Хэндлер для <see cref="DownloadUserImageByUserInfoIdQuery"/>
/// </summary>
internal class DownloadUserImageByUserInfoIdQueryHandler(
    IUserImageService userImageService) : IRequestHandler<DownloadUserImageByUserInfoIdQuery, UserImageDto>
{
    private readonly IUserImageService _userImageService = userImageService ?? throw new ArgumentNullException(nameof(userImageService));
    
    public async Task<UserImageDto> Handle(DownloadUserImageByUserInfoIdQuery request, CancellationToken cancellationToken)
    {
        var userImageDto = await _userImageService.DownloadUserImageByUserInfoId(request.UserInfoId, cancellationToken);
        return userImageDto;
    }
}