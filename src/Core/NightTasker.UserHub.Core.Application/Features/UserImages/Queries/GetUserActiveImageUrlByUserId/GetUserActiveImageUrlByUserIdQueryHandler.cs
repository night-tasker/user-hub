using MediatR;
using NightTasker.UserHub.Core.Application.Features.UserImages.Services.Contracts;

namespace NightTasker.UserHub.Core.Application.Features.UserImages.Queries.GetUserActiveImageUrlByUserId;

public class GetUserActiveImageUrlByUserIdQueryHandler(IUserImageService userImageService) : IRequestHandler<GetUserActiveImageUrlByUserIdQuery, string>
{
    private readonly IUserImageService _userImageService = userImageService ?? throw new ArgumentNullException(nameof(userImageService));

    public Task<string> Handle(GetUserActiveImageUrlByUserIdQuery request, CancellationToken cancellationToken)
    {
        return _userImageService.GetUserActiveImageUrlByUserId(request.UserId, cancellationToken);
    }
}