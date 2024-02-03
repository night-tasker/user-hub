using NightTasker.Common.Core.Abstractions;
using NightTasker.UserHub.Core.Domain.Repositories;

namespace NightTasker.UserHub.Core.Domain.Entities;

public class Organization : IEntityWithId<Guid>, IDateTimeOffsetModification
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public List<OrganizationUser> OrganizationUsers { get; set; } = null!;

    public DateTimeOffset CreatedDateTimeOffset { get; set; }
    
    public DateTimeOffset? UpdatedDateTimeOffset { get; set; }

    public Task<int> GetUsersCount(IOrganizationUserRepository organizationRepository, CancellationToken cancellationToken)
    {
        return organizationRepository.GetUsersCountInOrganization(Id, cancellationToken);
    }

    public void UpdateName(string name)
    {
        Name = name;
    }

    public void UpdateDescription(string description)
    {
        Description = description;
    }
}