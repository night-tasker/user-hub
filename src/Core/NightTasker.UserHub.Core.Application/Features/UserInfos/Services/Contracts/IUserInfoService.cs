using NightTasker.UserHub.Core.Application.Features.UserInfos.Models;

namespace NightTasker.UserHub.Core.Application.Features.UserInfos.Services.Contracts;

public interface IUserInfoService
{
    Task CreateUserInfo(CreateUserInfoDto createUserInfoDto, CancellationToken cancellationToken);

    Task UpdateUserInfo(UpdateUserInfoDto updateUserInfoDto, CancellationToken cancellationToken);
}