using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NightTasker.Common.Core.Identity.Contracts;
using NightTasker.UserHub.Core.Application.Features.OrganizationUserInvites.Commands.SendOrganizationUserInvite;
using NightTasker.UserHub.Presentation.WebApi.Requests.OrganizationUserInvites;

namespace NightTasker.UserHub.Presentation.WebApi.Controllers.V1;

[ApiController]
[Route("api/v1/organization-user-invites")]
[Authorize]
public class OrganizationUserInvitesController(
    ISender sender, IIdentityService identityService) : ControllerBase
{
    private readonly ISender _sender = sender ?? throw new ArgumentNullException(nameof(sender));
    private readonly IIdentityService _identityService = identityService
        ?? throw new ArgumentNullException(nameof(identityService));

    [HttpPost]
    public async Task<IActionResult> SendOrganizationUserInvite(
        [FromBody] SendOrganizationUserInviteRequest request, CancellationToken cancellationToken)
    {
        var command = new SendOrganizationUserInviteCommand(
            _identityService.CurrentUserId!.Value, request.InvitedUserId, request.OrganizationId, request.Message);
        await _sender.Send(command, cancellationToken);
        return Ok();
    }
}