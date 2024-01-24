namespace NightTasker.UserHub.Core.Application.Features.Organizations.Models;

/// <summary>
/// DTO для организации.
/// </summary>
/// <param name="Id">ИД организации.</param>
/// <param name="Name">Название организации.</param>
/// <param name="Description">Описание организации.</param>
public record OrganizationDto(Guid Id, string Name, string Description);