using MediatR;

namespace NightTasker.UserHub.Core.Application.Features.UserInfos.Commands.CreateUserInfo;

public record CreateUserInfoCommand(Guid Id, string UserName, string Email) : IRequest;