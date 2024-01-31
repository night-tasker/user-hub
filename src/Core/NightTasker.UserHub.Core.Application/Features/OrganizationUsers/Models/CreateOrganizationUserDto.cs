using NightTasker.UserHub.Core.Domain.Enums;

namespace NightTasker.UserHub.Core.Application.Features.OrganizationUsers.Models;

public record CreateOrganizationUserDto(Guid OrganizationId, Guid UserId, OrganizationUserRole Role);