using NightTasker.UserHub.Core.Domain.Entities;

namespace NightTasker.UserHub.Core.Application.Features.UserInfos.Models;

public record UpdateUserInfoDto(Guid Id, string? FirstName, string? MiddleName, string? LastName)
{
    public UserInfo MapFieldsToEntity(UserInfo userInfo)
    {
        userInfo.UpdateFirstName(FirstName);
        userInfo.UpdateMiddleName(MiddleName);
        userInfo.UpdateLastName(LastName);
        return userInfo;
    }
};