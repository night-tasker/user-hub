using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NightTasker.Common.Core.Identity.Contracts;
using NightTasker.UserHub.Core.Application.Features.Users.Commands.UpdateUser;
using NightTasker.UserHub.Core.Application.Features.Users.Models;
using NightTasker.UserHub.Core.Application.Features.Users.Queries.GetById;
using NightTasker.UserHub.Presentation.WebApi.Constants;
using NightTasker.UserHub.Presentation.WebApi.Endpoints;
using NightTasker.UserHub.Presentation.WebApi.Requests.User;

namespace NightTasker.UserHub.Presentation.WebApi.Controllers.V1;

/// <summary>
/// Контроллер для работы с <see cref="UserDto"/>
/// </summary>
[ApiController]
[Authorize]
[Route($"{ApiConstants.DefaultPrefix}/{ApiConstants.V1}/{UserEndpoints.UserResource}")]
public class UserController(
    IIdentityService identityService,
    ISender sender) : ControllerBase
{
    private readonly IIdentityService _identityService =
        identityService ?? throw new ArgumentNullException(nameof(identityService));
    private readonly ISender _sender = sender ?? throw new ArgumentNullException(nameof(sender));

    /// <summary>
    /// Эндпоинт для получения информации о пользователе.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Сведения о пользователе.</returns>
    [HttpGet(UserEndpoints.CurrentUser)]
    public async Task<ActionResult<UserDto>> GetCurrentUser(CancellationToken cancellationToken)
    {
        var currentUserId = _identityService.CurrentUserId!.Value;
        var query = new GetUserByIdQuery(currentUserId);
        var result = await _sender.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Эндпоинт для обновления информации о пользователе.
    /// </summary>
    /// <param name="request">Запрос на обновление сведений о пользователе.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    [HttpPut(UserEndpoints.UpdateUser)]
    public async Task<IActionResult> UpdateUser(
        UpdateCurrentUserRequest request,
        CancellationToken cancellationToken)
    {
        var currentUserId = _identityService.CurrentUserId!.Value;
        var command = new UpdateUserCommand(currentUserId, request.FirstName, request.MiddleName, request.LastName);
        await _sender.Send(command, cancellationToken);
        return Ok();
    }
}