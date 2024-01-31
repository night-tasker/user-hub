using MapsterMapper;
using MediatR;
using NightTasker.UserHub.Core.Application.Features.UserInfos.Models;
using NightTasker.UserHub.Core.Application.Features.UserInfos.Services.Contracts;

namespace NightTasker.UserHub.Core.Application.Features.UserInfos.Commands.CreateUserInfo;

internal class CreateUserInfoCommandHandler(
    IUserInfoService userInfoService,
    IMapper mapper) : IRequestHandler<CreateUserInfoCommand>
{
    private readonly IUserInfoService _userInfoService =
        userInfoService ?? throw new ArgumentNullException(nameof(userInfoService));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    
    public async Task Handle(CreateUserInfoCommand request, CancellationToken cancellationToken)
    {
        var createUserInfoDto = _mapper.Map<CreateUserInfoDto>(request);
        await _userInfoService.CreateUserInfo(createUserInfoDto, cancellationToken);
    }
}