using Microsoft.EntityFrameworkCore;
using NightTasker.Common.Core.Persistence;
using NightTasker.Common.Core.Persistence.Repository;
using NightTasker.UserHub.Core.Application.ApplicationContracts.Repository;
using NightTasker.UserHub.Core.Application.Exceptions.NotFound;
using NightTasker.UserHub.Core.Domain.Entities;
using NightTasker.UserHub.Core.Domain.Enums;

namespace NightTasker.UserHub.Infrastructure.Persistence.Repository;

/// <inheritdoc cref="IOrganizationUserRepository"/>
public class OrganizationUserRepository(ApplicationDbSet<OrganizationUser, Guid> dbSet)
    : BaseRepository<OrganizationUser, Guid>(dbSet), IOrganizationUserRepository
{
    /// <inheritdoc />
    public async Task<OrganizationUserRole> GetUserOrganizationRole(
        Guid organizationId, Guid userId, CancellationToken cancellationToken)
    {
        var organizationUser = await Entities
            .SingleOrDefaultAsync(x => 
                x.OrganizationId == organizationId && x.UserId == userId, cancellationToken);
        
        if(organizationUser is null)
        {
            throw new OrganizationUserNotFoundException(organizationId, userId);
        }
        
        return organizationUser.Role;
    }
}