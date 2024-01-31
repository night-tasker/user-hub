using NightTasker.Common.Core.Abstractions;
using NightTasker.UserHub.Core.Domain.Enums;

namespace NightTasker.UserHub.Core.Domain.Entities;

public class OrganizationUser : IEntity
{
    public OrganizationUserRole Role { get; set; }

    public Guid UserId { get; set; }

    public UserInfo UserInfo { get; set; } = null!;

    public Guid OrganizationId { get; set; }

    public Organization Organization { get; set; } = null!;
}