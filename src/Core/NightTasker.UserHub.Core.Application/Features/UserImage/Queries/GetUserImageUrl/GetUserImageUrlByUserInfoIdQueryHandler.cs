using MediatR;
using NightTasker.UserHub.Core.Application.Features.UserImage.Services.Contracts;

namespace NightTasker.UserHub.Core.Application.Features.UserImage.Queries.GetUserImageUrl;

/// <summary>
/// Хэндлер для <see cref="GetUserImageUrlByUserInfoIdQuery"/>
/// </summary>
public class GetUserImageUrlByUserInfoIdQueryHandler(IUserImageService userImageService) : IRequestHandler<GetUserImageUrlByUserInfoIdQuery, string>
{
    private readonly IUserImageService _userImageService = userImageService ?? throw new ArgumentNullException(nameof(userImageService));

    public Task<string> Handle(GetUserImageUrlByUserInfoIdQuery request, CancellationToken cancellationToken)
    {
        return _userImageService.GetUserImageUrlByUserInfoId(request.UserInfoId, cancellationToken);
    }
}