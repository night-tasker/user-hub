using MediatR;
using NightTasker.UserHub.Core.Application.Features.Users.Models;

namespace NightTasker.UserHub.Core.Application.Features.Users.Commands.CreateUser;

public record CreateUserCommand(Guid UserId, string UserName, string Email) : IRequest
{
    public CreateUserDto ToCreateUserDto()
    {
        return new CreateUserDto(UserId, UserName, Email);
    }
}