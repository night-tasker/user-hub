using NightTasker.UserHub.Core.Domain.Entities;

namespace NightTasker.UserHub.Core.Application.Features.Users.Models;

public record UpdateUserDto(Guid Id, string? FirstName, string? MiddleName, string? LastName)
{
    public User MapFieldsToEntity(User user)
    {
        user.UpdateFirstName(FirstName);
        user.UpdateMiddleName(MiddleName);
        user.UpdateLastName(LastName);
        return user;
    }
};