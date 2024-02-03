using MediatR;
using NightTasker.UserHub.Core.Application.Features.UserInfos.Services.Contracts;

namespace NightTasker.UserHub.Core.Application.Features.UserInfos.Commands.UpdateUserInfo;

internal class UpdateUserInfoCommandHandler(
    IUserInfoService userInfoService) : IRequestHandler<UpdateUserInfoCommand, Unit>
{
    private readonly IUserInfoService _userInfoService = userInfoService ?? throw new ArgumentNullException(nameof(userInfoService));

    public async Task<Unit> Handle(UpdateUserInfoCommand request, CancellationToken cancellationToken)
    {
        var updateUserInfoDto = request.ToUpdateUserInfoDto();
        await _userInfoService.UpdateUserInfo(updateUserInfoDto, cancellationToken);
        return Unit.Value;
    }
}