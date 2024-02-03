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
    public static UserInfoDto FromEntity(User user)
    {
        return new UserInfoDto(
            user.Id,
            user.UserName,
            user.Email,
            user.FirstName,
            user.MiddleName,
            user.LastName);
    }
};