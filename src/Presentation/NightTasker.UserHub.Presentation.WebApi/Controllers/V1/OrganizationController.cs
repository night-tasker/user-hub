using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NightTasker.Common.Core.Identity.Contracts;
using NightTasker.UserHub.Core.Application.Features.Organizations.Commands.CreateOrganizationAsUser;
using NightTasker.UserHub.Core.Application.Features.Organizations.Commands.RemoveOrganizationAsUser;
using NightTasker.UserHub.Core.Application.Features.Organizations.Commands.UpdateOrganizationAsUser;
using NightTasker.UserHub.Core.Application.Features.Organizations.Models;
using NightTasker.UserHub.Core.Application.Features.Organizations.Models.Search;
using NightTasker.UserHub.Core.Application.Features.Organizations.Queries.GetOrganizationById;
using NightTasker.UserHub.Core.Application.Features.Organizations.Queries.GetUserOrganizations;
using NightTasker.UserHub.Core.Application.Features.Organizations.Queries.SearchOrganizationsAsUser;
using NightTasker.UserHub.Core.Application.Features.OrganizationUsers.Queries.GetOrganizationUserRole;
using NightTasker.UserHub.Core.Application.Models;
using NightTasker.UserHub.Core.Domain.Common.Search;
using NightTasker.UserHub.Core.Domain.Entities;
using NightTasker.UserHub.Presentation.WebApi.Constants;
using NightTasker.UserHub.Presentation.WebApi.Endpoints;
using NightTasker.UserHub.Presentation.WebApi.Requests.Organization;
using NightTasker.UserHub.Presentation.WebApi.Responses.Organization;

namespace NightTasker.UserHub.Presentation.WebApi.Controllers.V1;

/// <summary>
/// Контроллер для работы с организациями (<see cref="Organization"/>).
/// </summary>
[ApiController]
[Route($"{ApiConstants.DefaultPrefix}/{ApiConstants.V1}/{OrganizationEndpoints.BaseResource}")]
[Authorize]
public class OrganizationController(
    IMediator mediator,
    IIdentityService identityService) : ControllerBase
{
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    private readonly IIdentityService _identityService =
        identityService ?? throw new ArgumentNullException(nameof(identityService));

    /// <summary>
    /// Эндпоинт для получения организации по идентификатору.
    /// </summary>
    /// <param name="organizationId">ИД организации.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Организация.</returns>
    [HttpGet(OrganizationEndpoints.GetById)]
    public async Task<ActionResult<OrganizationWithInfoDto>> GetOrganizationById(
        [FromRoute] Guid organizationId, CancellationToken cancellationToken)
    {
        var currentUserId = _identityService.CurrentUserId!.Value;
        var query = new GetOrganizationByIdAsUserQuery(organizationId, currentUserId);
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }
    
    /// <summary>
    /// Эндпоинт для получения организаций пользователя.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Список организаций.</returns>
    
    [HttpGet]
    public async Task<IActionResult> GetOrganizations(CancellationToken cancellationToken)
    {
        var currentUserId = _identityService.CurrentUserId!.Value;
        var query = new GetUserOrganizationsQuery(currentUserId);
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Эндпоинт для получения роли пользователя в организации.
    /// </summary>
    /// <param name="organizationId">ИД организации.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Роль пользователя в организации.</returns>
    [HttpGet(OrganizationEndpoints.GetUserOrganizationRole)]
    public async Task<IActionResult> GetOrganizationUserRole(
        [FromRoute] Guid organizationId, CancellationToken cancellationToken)
    {
        var currentUserId = _identityService.CurrentUserId!.Value;
        var query = new GetOrganizationUserRoleQuery(OrganizationId: organizationId, UserId: currentUserId);
        var result = await _mediator.Send(query, cancellationToken);
        var response = new GetOrganizationUserRoleResponse(result);
        return Ok(response);
    }
    
    /// <summary>
    /// Эндпоинт для поиска организаций.
    /// </summary>
    /// <param name="request">Запрос на поиск организаций.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Результат поиска организаций.</returns>
    [HttpPost(OrganizationEndpoints.SearchOrganizations)]
    public async Task<ActionResult<SearchResult<OrganizationDto>>> SearchOrganizations(
        [FromBody] SearchOrganizationsRequest request,
        CancellationToken cancellationToken)
    {
        var currentUserId = _identityService.CurrentUserId!.Value;
        var searchCriteria = new OrganizationsSearchCriteria(request.Filter, request.Sorter, request.Paging);
        var query = new SearchOrganizationsAsUserQuery(currentUserId, searchCriteria);
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }
    
    /// <summary>
    /// Эндпоинт для создания организации.
    /// </summary>
    /// <param name="request">Запрос на создание организации.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Идентификатор созданной организации.</returns>
    [HttpPost]
    public async Task<IActionResult> CreateOrganization(
        [FromBody] CreateOrganizationRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateOrganizationAsUserCommand(
            Name: request.Name, 
            Description: request.Description,
            UserId: _identityService.CurrentUserId!.Value);
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Эндпоинт для обновления организации.
    /// </summary>
    /// <param name="organizationId">ИД организации.</param>
    /// <param name="request">Запрос на обновление организации.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    [HttpPut(OrganizationEndpoints.UpdateOrganization)]
    public async Task<IActionResult> UpdateOrganization(
        [FromRoute] Guid organizationId,
        [FromBody] UpdateOrganizationDto request,
        CancellationToken cancellationToken)
    {
        var userId = _identityService.CurrentUserId!.Value;
        var command = new UpdateOrganizationAsUserCommand(userId, organizationId, request);
        await _mediator.Send(command, cancellationToken);
        return Ok();
    }
    
    /// <summary>
    /// Эндпоинт для удаления организации.
    /// </summary>
    /// <param name="organizationId">ИД организации.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    [HttpDelete(OrganizationEndpoints.DeleteOrganization)]
    public async Task<IActionResult> DeleteOrganization(
        [FromRoute] Guid organizationId,
        CancellationToken cancellationToken)
    {
        var userId = _identityService.CurrentUserId!.Value;
        var command = new RemoveOrganizationAsUserCommand(userId, organizationId);
        await _mediator.Send(command, cancellationToken);
        return Ok();
    }
}