using MediatR;
using NightTasker.UserHub.Core.Application.Features.UserInfos.Models;

namespace NightTasker.UserHub.Core.Application.Features.UserInfos.Commands.CreateUserInfo;

public record CreateUserInfoCommand(Guid UserId, string UserName, string Email) : IRequest
{
    public CreateUserInfoDto ToCreateUserInfoDto()
    {
        return new CreateUserInfoDto(UserId, UserName, Email);
    }
}