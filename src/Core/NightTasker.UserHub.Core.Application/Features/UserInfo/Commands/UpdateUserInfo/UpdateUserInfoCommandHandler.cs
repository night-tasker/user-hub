using MapsterMapper;
using MediatR;
using NightTasker.UserHub.Core.Application.Features.UserInfo.Models;
using NightTasker.UserHub.Core.Application.Features.UserInfo.Services.Contracts;

namespace NightTasker.UserHub.Core.Application.Features.UserInfo.Commands.UpdateUserInfo;

/// <summary>
/// Хэндлер для <see cref="UpdateUserInfoCommand"/>
/// </summary>
public class UpdateUserInfoCommandHandler(
    IUserInfoService userInfoService,
    IMapper mapper) : IRequestHandler<UpdateUserInfoCommand, Unit>
{
    private readonly IUserInfoService _userInfoService = userInfoService ?? throw new ArgumentNullException(nameof(userInfoService));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    public async Task<Unit> Handle(UpdateUserInfoCommand request, CancellationToken cancellationToken)
    {
        var updateUserInfoDto = _mapper.Map<UpdateUserInfoDto>(request);
        await _userInfoService.UpdateUserInfoWithSaving(updateUserInfoDto, cancellationToken);
        return Unit.Value;
    }
}