using NightTasker.UserHub.Core.Domain.Entities;

namespace NightTasker.UserHub.Core.Application.Features.Users.Models;

public record UserDto(
    Guid Id,
    string? UserName,
    string? Email,
    string? FirstName,
    string? MiddleName,
    string? LastName)
{
    public static UserDto FromEntity(User user)
    {
        return new UserDto(
            user.Id,
            user.UserName,
            user.Email,
            user.FirstName,
            user.MiddleName,
            user.LastName);
    }
};