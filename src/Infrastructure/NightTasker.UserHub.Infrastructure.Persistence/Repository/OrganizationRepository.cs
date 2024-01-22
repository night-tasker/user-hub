using NightTasker.Common.Core.Persistence;
using NightTasker.Common.Core.Persistence.Repository;
using NightTasker.UserHub.Core.Application.ApplicationContracts.Repository;
using NightTasker.UserHub.Core.Domain.Entities;

namespace NightTasker.UserHub.Infrastructure.Persistence.Repository;

/// <inheritdoc cref="IOrganizationRepository"/>
public class OrganizationRepository(ApplicationDbSet<Organization, Guid> dbSet)
    : BaseRepository<Organization, Guid>(dbSet), IOrganizationRepository
{
    
}