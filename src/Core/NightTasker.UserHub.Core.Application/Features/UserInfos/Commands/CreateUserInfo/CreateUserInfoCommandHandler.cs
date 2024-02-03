using MediatR;
using NightTasker.UserHub.Core.Application.Features.UserInfos.Services.Contracts;

namespace NightTasker.UserHub.Core.Application.Features.UserInfos.Commands.CreateUserInfo;

internal class CreateUserInfoCommandHandler(
    IUserInfoService userInfoService) : IRequestHandler<CreateUserInfoCommand>
{
    private readonly IUserInfoService _userInfoService =
        userInfoService ?? throw new ArgumentNullException(nameof(userInfoService));
    
    public async Task Handle(CreateUserInfoCommand request, CancellationToken cancellationToken)
    {
        var createUserInfoDto = request.ToCreateUserInfoDto();
        await _userInfoService.CreateUserInfo(createUserInfoDto, cancellationToken);
    }
}