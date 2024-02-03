using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NightTasker.Common.Core.Identity.Contracts;
using NightTasker.UserHub.Core.Application.Features.UserInfos.Commands.UpdateUserInfo;
using NightTasker.UserHub.Core.Application.Features.UserInfos.Models;
using NightTasker.UserHub.Core.Application.Features.UserInfos.Queries.GetById;
using NightTasker.UserHub.Presentation.WebApi.Constants;
using NightTasker.UserHub.Presentation.WebApi.Endpoints;
using NightTasker.UserHub.Presentation.WebApi.Requests.UserInfo;

namespace NightTasker.UserHub.Presentation.WebApi.Controllers.V1;

/// <summary>
/// Контроллер для работы с <see cref="UserInfoDto"/>
/// </summary>
[ApiController]
[Authorize]
[Route($"{ApiConstants.DefaultPrefix}/{ApiConstants.V1}/{UserInfoEndpoints.UserInfoResource}")]
public class UserInfoController(
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
    [HttpGet(UserInfoEndpoints.CurrentUserInfo)]
    public async Task<ActionResult<UserInfoDto>> GetCurrentUserInfo(CancellationToken cancellationToken)
    {
        var currentUserId = _identityService.CurrentUserId!.Value;
        var query = new GetUserInfoByIdQuery(currentUserId);
        var result = await _sender.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Эндпоинт для обновления информации о пользователе.
    /// </summary>
    /// <param name="request">Запрос на обновление сведений о пользователе.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    [HttpPut(UserInfoEndpoints.UpdateUserInfo)]
    public async Task<IActionResult> UpdateUserInfo(
        UpdateCurrentUserInfoRequest request,
        CancellationToken cancellationToken)
    {
        var currentUserId = _identityService.CurrentUserId!.Value;
        var command = new UpdateUserInfoCommand(currentUserId, request.FirstName, request.MiddleName, request.LastName);
        await _sender.Send(command, cancellationToken);
        return Ok();
    }
}