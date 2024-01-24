using NightTasker.Common.Core.Persistence;
using NightTasker.Common.Core.Persistence.Repository;
using NightTasker.UserHub.Core.Application.ApplicationContracts.Repository;
using NightTasker.UserHub.Core.Domain.Entities;

namespace NightTasker.UserHub.Infrastructure.Persistence.Repository;

/// <inheritdoc cref="IOrganizationUserRepository"/>
public class OrganizationUserRepository(ApplicationDbSet<OrganizationUser, Guid> dbSet)
    : BaseRepository<OrganizationUser, Guid>(dbSet), IOrganizationUserRepository;