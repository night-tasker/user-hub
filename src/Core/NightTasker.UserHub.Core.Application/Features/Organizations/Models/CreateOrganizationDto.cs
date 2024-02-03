using NightTasker.UserHub.Core.Domain.Entities;

namespace NightTasker.UserHub.Core.Application.Features.Organizations.Models;

public record CreateOrganizationDto(string Name, string? Description)
{
    public Organization ToEntity()
    {
        return new Organization
        {
            Name = Name,
            Description = Description
        };
    }
};