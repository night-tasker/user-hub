using NightTasker.UserHub.Core.Domain.Entities;

namespace NightTasker.UserHub.Core.Application.Features.UserInfos.Models;

public record CreateUserInfoDto(Guid UserId, string UserName, string Email)
{
    public UserInfo ToEntity()
    {
        return new UserInfo
        {
            Id = UserId,
            UserName = UserName,
            Email = Email
        };
    }
};