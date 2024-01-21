using MediatR;
using NightTasker.UserHub.Core.Application.Features.UserImage.Models;
using NightTasker.UserHub.Core.Application.Features.UserImage.Services.Contracts;

namespace NightTasker.UserHub.Core.Application.Features.UserImage.Queries.GetUserImagesWithUrlByUserInfoId;

/// <summary>
/// Хэндлер для <see cref="GetUserImagesWithUrlByUserInfoIdQuery"/>
/// </summary>
internal class GetUserImagesWithUrlByUserInfoIdQueryHandler(IUserImageService userImageService)
    : IRequestHandler<GetUserImagesWithUrlByUserInfoIdQuery, IReadOnlyCollection<UserImageWithUrlDto>>
{
    private readonly IUserImageService _userImageService = 
        userImageService ?? throw new ArgumentNullException(nameof(userImageService));

    public Task<IReadOnlyCollection<UserImageWithUrlDto>> Handle(
        GetUserImagesWithUrlByUserInfoIdQuery request, CancellationToken cancellationToken)
    {
        return _userImageService.GetUserImagesByUserInfoId(request.UserInfoId, cancellationToken);
    }
}