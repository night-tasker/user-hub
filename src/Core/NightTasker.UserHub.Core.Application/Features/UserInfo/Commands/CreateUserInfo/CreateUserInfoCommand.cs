using MediatR;

namespace NightTasker.UserHub.Core.Application.Features.UserInfo.Commands.CreateUserInfo;

/// <summary>
/// Запрос на создание <see cref="UserInfo"/>.
/// </summary>
/// <param name="Id">ИД пользователя.</param>
/// <param name="UserName">Имя пользователя.</param>
/// <param name="Email">Адрес электронной почты.</param>
public record CreateUserInfoCommand(Guid Id, string UserName, string Email) : IRequest;