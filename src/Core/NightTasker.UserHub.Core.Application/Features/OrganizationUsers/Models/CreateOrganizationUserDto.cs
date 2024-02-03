using NightTasker.UserHub.Core.Domain.Entities;
using NightTasker.UserHub.Core.Domain.Enums;

namespace NightTasker.UserHub.Core.Application.Features.OrganizationUsers.Models;

public record CreateOrganizationUserDto(Guid OrganizationId, Guid UserId, OrganizationUserRole Role)
{
    public OrganizationUser ToEntity()
    {
        return new OrganizationUser
        {
            OrganizationId = OrganizationId,
            UserId = UserId,
            Role = Role
        };
    }
};