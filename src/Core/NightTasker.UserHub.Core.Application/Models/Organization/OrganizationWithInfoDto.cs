namespace NightTasker.UserHub.Core.Application.Models.Organization;

public record OrganizationWithInfoDto(
    Guid Id, string? Name, string? Description, int UsersCount);