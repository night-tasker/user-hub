namespace NightTasker.UserHub.Core.Application.Features.UserInfos.Models;

public record UpdateUserInfoDto(Guid Id, string FirstName, string MiddleName, string LastName);