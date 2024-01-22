using Mapster;
using NightTasker.UserHub.Core.Application.Features.UserInfos.Commands.CreateUserInfo;
using NightTasker.UserHub.Core.Application.Features.UserInfos.Models;

namespace NightTasker.UserHub.Core.Application.Features.UserInfos.Profiles;

public class UserInfoProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<CreateUserInfoCommand, CreateUserInfoDto>();
        config.ForType<CreateUserInfoDto, Domain.Entities.UserInfo>();
        config.ForType<Domain.Entities.UserInfo, UserInfoDto>();
    }
}