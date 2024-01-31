using NightTasker.UserHub.Core.Application.Features.UserImages.Models;

namespace NightTasker.UserHub.Core.Application.Features.UserImages.Services.Contracts;

public interface IUserImageService
{
    Task<Guid> CreateUserImage(CreateUserImageDto createUserImageDto, CancellationToken cancellationToken);

    Task<UserImageWithStreamDto> DownloadActiveUserImageByUserInfoId(Guid userInfoId, CancellationToken cancellationToken);

    Task<string> GetUserActiveImageUrlByUserInfoId(Guid userInfoId, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<UserImageWithUrlDto>> GetUserImagesByUserInfoId(
        Guid userInfoId, CancellationToken cancellationToken);

    Task RemoveUserImageById(Guid userInfoId, Guid userImageId, CancellationToken cancellationToken);

    Task SetActiveUserImageForUserInfoId(
        Guid userInfoId, Guid userImageId, CancellationToken cancellationToken);
}