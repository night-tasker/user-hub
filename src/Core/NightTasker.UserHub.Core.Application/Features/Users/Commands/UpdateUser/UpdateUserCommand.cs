using MediatR;
using NightTasker.UserHub.Core.Application.Features.Users.Models;

namespace NightTasker.UserHub.Core.Application.Features.Users.Commands.UpdateUser;

public record UpdateUserCommand(Guid Id, string? FirstName, string? MiddleName, string? LastName) : IRequest<Unit>
{
    public UpdateUserDto ToUpdateUserDto()
    {
        return new UpdateUserDto(Id, FirstName, MiddleName, LastName);
    }
}