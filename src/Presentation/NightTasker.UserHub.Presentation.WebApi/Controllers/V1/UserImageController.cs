using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NightTasker.Common.Core.Identity.Contracts;
using NightTasker.UserHub.Core.Application.Features.UserImage.Commands.RemoveUserImage;
using NightTasker.UserHub.Core.Application.Features.UserImage.Commands.SetActiveUserImage;
using NightTasker.UserHub.Core.Application.Features.UserImage.Commands.UploadUserImage;
using NightTasker.UserHub.Core.Application.Features.UserImage.Queries.DownloadByUserId;
using NightTasker.UserHub.Core.Application.Features.UserImage.Queries.GetUserActiveImageUrlByUserInfoId;
using NightTasker.UserHub.Core.Application.Features.UserImage.Queries.GetUserImagesWithUrlByUserInfoId;
using NightTasker.UserHub.Presentation.WebApi.Constants;
using NightTasker.UserHub.Presentation.WebApi.Endpoints;
using NightTasker.UserHub.Presentation.WebApi.Responses.UserImage;

namespace NightTasker.UserHub.Presentation.WebApi.Controllers.V1;

/// <summary>
/// Контроллер для работы с фото пользователей.
/// </summary>
[ApiController]
[Route($"{ApiConstants.DefaultPrefix}/{ApiConstants.V1}/{UserImageEndpoints.UserImageResource}")]
[Authorize]
public class UserImageController(
    IIdentityService identityService,
    IMediator mediator) : ControllerBase
{
    private readonly IIdentityService _identityService =
        identityService ?? throw new ArgumentNullException(nameof(identityService));
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

    /// <summary>
    /// Эндпоинт для получения фотографии пользователя.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Фото пользователя.</returns>
    [HttpGet(UserImageEndpoints.CurrentUserImage)]
    public async Task<IActionResult> GetCurrentUserImage(
        CancellationToken cancellationToken)
    {
        var currentUserId = _identityService.CurrentUserId!.Value;
        var query = new DownloadUserImageByUserInfoIdQuery(currentUserId);
        var result = await _mediator.Send(query, cancellationToken);
        return File(result.Stream, result.ContentType, $"{result.FileName}.{result.Extension}");
    }

    /// <summary>
    /// Эндпоинт для загрузки фотографии текущего пользователя.
    /// </summary>
    /// <param name="file">Файл.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    [HttpPost(UserImageEndpoints.UploadCurrentUserImage)]
    public async Task<IActionResult> UploadCurrentUserImage(
        [FromForm] IFormFile file,
        CancellationToken cancellationToken)
    {
        var currentUserId = _identityService.CurrentUserId!.Value;
        var command = new UploadUserImageCommand(currentUserId, file.OpenReadStream(), file.FileName, file.ContentType, file.Length);
        await _mediator.Send(command, cancellationToken);
        return Ok();
    }

    /// <summary>
    /// Получить ссылку на активную фотографию текущего пользователя.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Ссылка на активную фотографию.</returns>
    [HttpGet(UserImageEndpoints.GetCurrentUserActiveImageUrl)]
    public async Task<ActionResult<GetCurrentUserActiveImageUrlResponse>> GetCurrentUserActiveImageUrl(
        CancellationToken cancellationToken)
    {
        var currentUserId = _identityService.CurrentUserId!.Value;
        var query = new GetUserActiveImageUrlByUserInfoIdQuery(currentUserId);
        var result = await _mediator.Send(query, cancellationToken);
        var response = new GetCurrentUserActiveImageUrlResponse(result);
        return Ok(response);
    }
    
    /// <summary>
    /// Получить ссылки на фотографии текущего пользователя.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Ссылки на фотографии.</returns>
    [HttpGet(UserImageEndpoints.GetCurrentUserImagesUrl)]
    public async Task<ActionResult<GetCurrentUserImagesUrlResponse>> GetCurrentUserImagesUrl(
        CancellationToken cancellationToken)
    {
        var currentUserId = _identityService.CurrentUserId!.Value;
        var query = new GetUserImagesWithUrlByUserInfoIdQuery(currentUserId);
        var result = await _mediator.Send(query, cancellationToken);
        var response = new GetCurrentUserImagesUrlResponse(result);
        return Ok(response);
    }

    /// <summary>
    /// Эндпоинт для удаления фотографии текущего пользователя.
    /// </summary>
    /// <param name="userImageId">Идентификатор фотографии.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    [HttpDelete(UserImageEndpoints.RemoveCurrentUserImageById)]
    public async Task<IActionResult> RemoveCurrentUserImageById(
        Guid userImageId, CancellationToken cancellationToken)
    {
        var command = new RemoveUserImageCommand(userImageId);
        await _mediator.Send(command, cancellationToken);
        return Ok();
    }

    /// <summary>
    /// Эндпоинт для установки активной фотографии текущего пользователя.
    /// </summary>
    /// <param name="userImageId">Идентификатор фотографии.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    [HttpPost(UserImageEndpoints.SetActiveUserImage)]
    public async Task<IActionResult> SetActiveUserImage(
        [FromRoute] Guid userImageId, CancellationToken cancellationToken)
    {
        var currentUserId = _identityService.CurrentUserId!.Value;
        var command = new SetActiveImageForUserCommand(currentUserId, userImageId);
        await _mediator.Send(command, cancellationToken);
        return Ok();
    }
}