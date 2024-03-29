﻿using NightTasker.UserHub.Core.Domain.Entities;
using NightTasker.UserHub.Core.Domain.Enums;

namespace NightTasker.UserHub.Core.Application.Features.OrganizationUsers.Models;

public record CreateOrganizationUserDto(Guid OrganizationId, Guid UserId, OrganizationUserRole Role)
{
    public OrganizationUser ToEntity()
    {
        return OrganizationUser.CreateInstance(OrganizationId, UserId, Role);
    }
}