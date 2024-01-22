﻿using NightTasker.Common.Core.Abstractions;

namespace NightTasker.UserHub.Core.Domain.Entities;

/// <summary>
/// Организация.
/// </summary>
public class Organization : IEntityWithId<Guid>, IDateTimeOffsetModification
{
    /// <inheritdoc />
    public Guid Id { get; set; }

    /// <summary>
    /// Название организации.
    /// </summary>
    public string? Name { get; set; }
    
    /// <summary>
    /// Список пользователей.
    /// </summary>
    public List<UserInfo> Users { get; set; } = null!;

    /// <summary>
    /// Список связей пользователя с организацией.
    /// </summary>
    public List<OrganizationUser> OrganizationUsers { get; set; } = null!;

    /// <inheritdoc />
    public DateTimeOffset CreatedDateTimeOffset { get; set; }
    
    /// <inheritdoc />
    public DateTimeOffset? UpdatedDateTimeOffset { get; set; }
}