namespace NightTasker.UserHub.Presentation.WebApi.Requests.OrganizationUserInvites;

public record SendOrganizationUserInviteRequest(
    Guid InvitedUserId, Guid OrganizationId, string? Message);