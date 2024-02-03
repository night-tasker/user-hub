using MediatR;
using NightTasker.UserHub.Core.Application.Features.UserImages.Models;
using NightTasker.UserHub.Core.Application.Features.UserImages.Services.Contracts;

namespace NightTasker.UserHub.Core.Application.Features.UserImages.Queries.GetUserImagesWithUrlByUserId;

internal class GetUserImagesWithUrlByUserIdQueryHandler(IUserImageService userImageService)
    : IRequestHandler<GetUserImagesWithUrlByUserIdQuery, IReadOnlyCollection<UserImageWithUrlDto>>
{
    private readonly IUserImageService _userImageService = 
        userImageService ?? throw new ArgumentNullException(nameof(userImageService));

    public Task<IReadOnlyCollection<UserImageWithUrlDto>> Handle(
        GetUserImagesWithUrlByUserIdQuery request, CancellationToken cancellationToken)
    {
        return _userImageService.GetUserImagesByUserId(request.UserId, cancellationToken);
    }
}