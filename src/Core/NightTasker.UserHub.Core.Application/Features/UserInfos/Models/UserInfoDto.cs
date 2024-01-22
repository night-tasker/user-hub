namespace NightTasker.UserHub.Core.Application.Features.UserInfos.Models;

/// <summary>
/// Dto сведений о пользователе.
/// </summary>
/// <param name="Id">ИД сведений о пользователе.</param>
/// <param name="UserName">Имя пользователя.</param>
/// <param name="Email">Email пользователя.</param>
/// <param name="FirstName">Имя.</param>
/// <param name="MiddleName">Отчество.</param>
/// <param name="LastName">Фамилия.</param>
public record UserInfoDto(
    Guid Id, 
    string UserName, 
    string Email, 
    string FirstName, 
    string MiddleName, 
    string LastName);