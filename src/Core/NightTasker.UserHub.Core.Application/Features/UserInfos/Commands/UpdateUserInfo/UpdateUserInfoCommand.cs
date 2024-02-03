using MediatR;
using NightTasker.UserHub.Core.Application.Features.UserInfos.Models;

namespace NightTasker.UserHub.Core.Application.Features.UserInfos.Commands.UpdateUserInfo;

public record UpdateUserInfoCommand(Guid Id, string? FirstName, string? MiddleName, string? LastName) : IRequest<Unit>
{
    public UpdateUserInfoDto ToUpdateUserInfoDto()
    {
        return new UpdateUserInfoDto(Id, FirstName, MiddleName, LastName);
    }
}