using NightTasker.UserHub.Core.Domain.Entities;

namespace NightTasker.UserHub.Core.Application.Features.Organizations.Models;

/// <summary>
/// DTO для создания организации (<see cref="Organization"/>).
/// </summary>
/// <param name="Name">Имя организации.</param>
/// <param name="Description">Описание организации.</param>
public record CreateOrganizationDto(string Name, string Description);