using Microsoft.EntityFrameworkCore;
using NightTasker.Common.Core.Persistence;
using NightTasker.UserHub.Core.Domain.Entities;
using NightTasker.UserHub.Infrastructure.Persistence;
using NightTasker.UserHub.Infrastructure.Persistence.Contracts;

namespace NightTasker.UserHub.Presentation.WebApi.Implementations;

public class ApplicationDataAccessor : IApplicationDataAccessor
{
    private readonly ApplicationDbContext _dbContext;
    
    public ApplicationDataAccessor(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        Organizations = new ApplicationDbSet<Organization, Guid>(_dbContext, GetOrganizations(_dbContext.Set<Organization>()));
        Users = new ApplicationDbSet<User, Guid>(_dbContext, GetUsers(_dbContext.Set<User>()));
        UserImages = new ApplicationDbSet<UserImage, Guid>(_dbContext, GetUserImages(_dbContext.Set<UserImage>()));
        OrganizationUsers = new ApplicationDbSet<OrganizationUser, Guid>(_dbContext, GetOrganizationUsers(_dbContext.Set<OrganizationUser>()));
        OrganizationUserInvites = new ApplicationDbSet<OrganizationUserInvite, Guid>(_dbContext, _dbContext.Set<OrganizationUserInvite>());
    }

    public ApplicationDbSet<User, Guid> Users { get; }

    public ApplicationDbSet<UserImage, Guid> UserImages { get; }
    
    public ApplicationDbSet<Organization, Guid> Organizations { get; }
    
    public ApplicationDbSet<OrganizationUser, Guid> OrganizationUsers { get; }
    
    public ApplicationDbSet<OrganizationUserInvite, Guid> OrganizationUserInvites { get; }

    private IQueryable<User> GetUsers(DbSet<User> query)
    {
        return query;
    }

    private IQueryable<UserImage> GetUserImages(DbSet<UserImage> query)
    {
        return query;
    }

    private IQueryable<Organization> GetOrganizations(DbSet<Organization> query)
    {
        return query;
    }

    private IQueryable<OrganizationUser> GetOrganizationUsers(DbSet<OrganizationUser> query)
    {
        return query;
    }

    public Task SaveChanges(CancellationToken cancellationToken)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}