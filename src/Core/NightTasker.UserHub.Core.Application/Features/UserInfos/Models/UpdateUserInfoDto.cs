namespace NightTasker.UserHub.Core.Application.Features.UserInfos.Models;

/// <summary>
/// Dto для обновления <see cref="UserInfos"/>.
/// </summary>
/// <param name="Id">ИД (UserId).</param>
/// <param name="FirstName">Имя.</param>
/// <param name="MiddleName">Отчество.</param>
/// <param name="LastName">Фамилия.</param>
public record UpdateUserInfoDto(Guid Id, string FirstName, string MiddleName, string LastName);