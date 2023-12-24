using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NightTasker.Common.Core.Identity.Contracts;
using NightTasker.UserHub.Core.Application.Features.UserInfo.Models;
using NightTasker.UserHub.Core.Application.Features.UserInfo.Queries.GetById;
using NightTasker.UserHub.Presentation.WebApi.Constants;
using NightTasker.UserHub.Presentation.WebApi.Endpoints;

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

    [HttpGet(UserInfoEndpoints.CurrentUserInfo)]
    public async Task<ActionResult<UserInfoDto>> GetCurrentUserInfo(CancellationToken cancellationToken)
    {
        var currentUserId = _identityService.CurrentUserId!.Value;
        var query = new GetUserInfoByIdQuery(currentUserId);
        var result = await _sender.Send(query, cancellationToken);
        return Ok(result);
    }
}