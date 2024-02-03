using NightTasker.UserHub.Core.Domain.Entities;

namespace NightTasker.UserHub.Core.Application.Features.Users.Models;

public record CreateUserDto(Guid UserId, string UserName, string Email)
{
    public User ToEntity()
    {
        return new User
        {
            Id = UserId,
            UserName = UserName,
            Email = Email
        };
    }
};