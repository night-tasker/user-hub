namespace NightTasker.UserHub.Core.Application.Features.UserInfos.Models;

public record UserInfoDto(
    Guid Id, 
    string UserName, 
    string Email, 
    string FirstName, 
    string MiddleName, 
    string LastName);