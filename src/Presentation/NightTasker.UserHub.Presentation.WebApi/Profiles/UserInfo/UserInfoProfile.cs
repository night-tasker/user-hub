using Mapster;
using NightTasker.UserHub.Core.Application.Features.UserInfos.Commands.UpdateUserInfo;
using NightTasker.UserHub.Core.Application.Features.UserInfos.Models;
using NightTasker.UserHub.Presentation.WebApi.Requests.UserInfo;

namespace NightTasker.UserHub.Presentation.WebApi.Profiles.UserInfo;

/// <summary>
/// Профиль маппинга для сведений пользователей.
/// </summary>
public class UserInfoProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<UpdateCurrentUserInfoRequest, UpdateUserInfoCommand>();
        config.ForType<UpdateUserInfoCommand, UpdateUserInfoDto>();
        config.ForType<UpdateUserInfoDto, Core.Domain.Entities.UserInfo>();
    }
}