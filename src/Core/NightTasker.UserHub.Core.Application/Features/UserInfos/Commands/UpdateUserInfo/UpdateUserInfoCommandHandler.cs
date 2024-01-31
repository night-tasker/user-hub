using MapsterMapper;
using MediatR;
using NightTasker.UserHub.Core.Application.Features.UserInfos.Models;
using NightTasker.UserHub.Core.Application.Features.UserInfos.Services.Contracts;

namespace NightTasker.UserHub.Core.Application.Features.UserInfos.Commands.UpdateUserInfo;

internal class UpdateUserInfoCommandHandler(
    IUserInfoService userInfoService,
    IMapper mapper) : IRequestHandler<UpdateUserInfoCommand, Unit>
{
    private readonly IUserInfoService _userInfoService = userInfoService ?? throw new ArgumentNullException(nameof(userInfoService));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    public async Task<Unit> Handle(UpdateUserInfoCommand request, CancellationToken cancellationToken)
    {
        var updateUserInfoDto = _mapper.Map<UpdateUserInfoDto>(request);
        await _userInfoService.UpdateUserInfo(updateUserInfoDto, cancellationToken);
        return Unit.Value;
    }
}