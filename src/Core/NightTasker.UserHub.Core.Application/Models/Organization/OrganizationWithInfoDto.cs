namespace NightTasker.UserHub.Core.Application.Models.Organization;

/// <summary>
/// Организация с информацией.
/// </summary>
/// <param name="Id">ИД организации.</param>
/// <param name="Name">Название.</param>
/// <param name="Description">Описание.</param>
/// <param name="UsersCount">Количество пользователей.</param>
public record OrganizationWithInfoDto(
    Guid Id, string? Name, string? Description, int UsersCount);