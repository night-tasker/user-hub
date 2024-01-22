using MediatR;
using NightTasker.UserHub.Core.Application.Features.UserImages.Services.Contracts;

namespace NightTasker.UserHub.Core.Application.Features.UserImages.Queries.GetUserActiveImageUrlByUserInfoId;

/// <summary>
/// Хэндлер для <see cref="GetUserActiveImageUrlByUserInfoIdQuery"/>
/// </summary>
public class GetUserActiveImageUrlByUserInfoIdQueryHandler(IUserImageService userImageService) : IRequestHandler<GetUserActiveImageUrlByUserInfoIdQuery, string>
{
    private readonly IUserImageService _userImageService = userImageService ?? throw new ArgumentNullException(nameof(userImageService));

    public Task<string> Handle(GetUserActiveImageUrlByUserInfoIdQuery request, CancellationToken cancellationToken)
    {
        return _userImageService.GetUserActiveImageUrlByUserInfoId(request.UserInfoId, cancellationToken);
    }
}