using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NightTasker.UserHub.Core.Application.Features.Organizations.Commands.CreateOrganization;
using NightTasker.UserHub.Core.Domain.Entities;
using NightTasker.UserHub.Presentation.WebApi.Constants;
using NightTasker.UserHub.Presentation.WebApi.Endpoints;
using NightTasker.UserHub.Presentation.WebApi.Requests.Organization;

namespace NightTasker.UserHub.Presentation.WebApi.Controllers.V1;

/// <summary>
/// Контроллер для работы с организациями (<see cref="Organization"/>).
/// </summary>
[ApiController]
[Route($"{ApiConstants.DefaultPrefix}/{ApiConstants.V1}/{UserImageEndpoints.UserImageResource}")]
[Authorize]
public class OrganizationController(
    IMediator mediator,
    IMapper mapper) : ControllerBase
{
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    /// <summary>
    /// Эндпоинт для создания организации.
    /// </summary>
    /// <param name="request">Запрос на создание организации.</param>
    [HttpPost]
    public async Task<IActionResult> CreateOrganization(
        [FromBody] CreateOrganizationRequest request,
        CancellationToken cancellationToken)
    {
        var command = _mapper.Map<CreateOrganizationCommand>(request);
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }
}