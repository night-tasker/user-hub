namespace NightTasker.UserHub.Core.Application.Features.UserInfo.Models;

/// <summary>
/// Dto для создания <see cref="UserInfo"/>.
/// </summary>
/// <param name="Id">ИД (UserId).</param>
/// <param name="UserName">Имя пользователя.</param>
public record CreateUserInfoDto(Guid Id, string UserName);