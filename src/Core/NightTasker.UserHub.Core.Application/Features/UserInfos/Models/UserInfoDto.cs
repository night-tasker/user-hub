using NightTasker.UserHub.Core.Domain.Entities;

namespace NightTasker.UserHub.Core.Application.Features.UserInfos.Models;

public record UserInfoDto(
    Guid Id,
    string? UserName,
    string? Email,
    string? FirstName,
    string? MiddleName,
    string? LastName)
{
    public static UserInfoDto FromEntity(UserInfo userInfo)
    {
        return new UserInfoDto(
            userInfo.Id,
            userInfo.UserName,
            userInfo.Email,
            userInfo.FirstName,
            userInfo.MiddleName,
            userInfo.LastName);
    }
};