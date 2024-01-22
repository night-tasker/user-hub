using MediatR;
using NightTasker.UserHub.Core.Application.Features.UserInfos.Models;
using NightTasker.UserHub.Core.Application.Features.UserInfos.Services.Contracts;

namespace NightTasker.UserHub.Core.Application.Features.UserInfos.Queries.GetById;

/// <summary>
/// Хэндлер для <see cref="GetUserInfoByIdQuery"/>
/// </summary>
public class GetUserInfoByIdQueryHandler(IUserInfoService userInfoService) : IRequestHandler<GetUserInfoByIdQuery, UserInfoDto>
{
    private readonly IUserInfoService _userInfoService =
        userInfoService ?? throw new ArgumentNullException(nameof(userInfoService));

    public Task<UserInfoDto> Handle(GetUserInfoByIdQuery request, CancellationToken cancellationToken)
    {
        return _userInfoService.GetUserInfoById(request.UserInfoId, cancellationToken);
    }
}