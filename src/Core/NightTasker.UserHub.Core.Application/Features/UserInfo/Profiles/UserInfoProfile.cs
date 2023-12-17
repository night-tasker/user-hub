using Mapster;
using NightTasker.UserHub.Core.Application.Features.UserInfo.Commands.CreateUserInfo;
using NightTasker.UserHub.Core.Application.Features.UserInfo.Models;

namespace NightTasker.UserHub.Core.Application.Features.UserInfo.Profiles;

public class UserInfoProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<CreateUserInfoCommand, CreateUserInfoDto>();
        config.ForType<CreateUserInfoDto, Domain.Entities.UserInfo>();
    }
}