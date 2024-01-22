namespace NightTasker.UserHub.Core.Application.Features.UserInfos.Models;

/// <summary>
/// Dto для создания <see cref="UserInfos"/>.
/// </summary>
/// <param name="Id">ИД (UserId).</param>
/// <param name="UserName">Имя пользователя.</param>
/// <param name="Email">Адрес электронной почты.</param>
public record CreateUserInfoDto(Guid Id, string UserName, string Email);