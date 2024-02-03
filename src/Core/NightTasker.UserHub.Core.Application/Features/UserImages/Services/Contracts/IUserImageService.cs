using NightTasker.UserHub.Core.Application.Features.UserImages.Models;

namespace NightTasker.UserHub.Core.Application.Features.UserImages.Services.Contracts;

public interface IUserImageService
{
    Task<Guid> CreateUserImage(CreateUserImageDto createUserImageDto, CancellationToken cancellationToken);

    Task<UserImageWithStreamDto> DownloadActiveUserImageByUserId(Guid userId, CancellationToken cancellationToken);

    Task<string> GetUserActiveImageUrlByUserId(Guid userId, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<UserImageWithUrlDto>> GetUserImagesByUserId(
        Guid userId, CancellationToken cancellationToken);

    Task RemoveUserImageById(Guid userId, Guid userImageId, CancellationToken cancellationToken);

    Task SetActiveUserImageForUser(
        Guid userId, Guid userImageId, CancellationToken cancellationToken);
}