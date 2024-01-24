using MediatR;

namespace NightTasker.UserHub.Core.Application.Features.Organizations.Commands.CreateOrganization;

/// <summary>
/// Команда на создание организации.
/// </summary>
/// <param name="Name">Имя организации.</param>
/// <param name="Description">Описание организации.</param>
/// <param name="UserId">ИД пользователя.</param>
public record CreateOrganizationAsUserCommand(string Name, string? Description, Guid UserId) : IRequest<Guid>;