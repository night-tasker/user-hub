using MediatR;

namespace NightTasker.UserHub.Core.Application.Features.Organizations.Commands.CreateOrganization;

/// <summary>
/// Команда на создание организации.
/// </summary>
/// <param name="Name">Имя организации.</param>
public record CreateOrganizationCommand(string Name) : IRequest<Guid>;