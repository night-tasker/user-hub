using Microsoft.EntityFrameworkCore;
using NightTasker.Common.Core.Identity.Contracts;
using NightTasker.Common.Core.Persistence;
using NightTasker.UserHub.Core.Domain.Entities;
using NightTasker.UserHub.Infrastructure.Persistence;
using NightTasker.UserHub.Infrastructure.Persistence.Contracts;

namespace NightTasker.UserHub.Presentation.WebApi.Implementations;

/// <inheritdoc /> 
public class ApplicationDbAccessor : IApplicationDbAccessor
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IIdentityService _identityService;
    
    public ApplicationDbAccessor(ApplicationDbContext dbContext, IIdentityService identityService)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
        Organizations = new ApplicationDbSet<Organization, Guid>(_dbContext, GetOrganizations(_dbContext.Set<Organization>()));
        UserInfos = new ApplicationDbSet<UserInfo, Guid>(_dbContext, GetUserInfos(_dbContext.Set<UserInfo>()));
        UserImages = new ApplicationDbSet<UserImage, Guid>(_dbContext, GetUserImages(_dbContext.Set<UserImage>()));
    }

    /// <inheritdoc />
    public ApplicationDbSet<UserInfo, Guid> UserInfos { get; }

    public ApplicationDbSet<UserImage, Guid> UserImages { get; }
    
    public ApplicationDbSet<Organization, Guid> Organizations { get; }

    private IQueryable<UserInfo> GetUserInfos(DbSet<UserInfo> query)
    {
        if (_identityService.IsSystem)
        {
            return query;
        }

        if (_identityService.IsAuthenticated)
        {
            var currentUserId = _identityService.CurrentUserId!.Value;
            return query.Where(x => x.Id == currentUserId);
        }

        return EmptyQuery(query);
    }

    private IQueryable<UserImage> GetUserImages(DbSet<UserImage> query)
    {
        if (_identityService.IsSystem)
        {
            return query;
        }

        if (_identityService.IsAuthenticated)
        {
            var currentUserId = _identityService.CurrentUserId!.Value;
            return query.Where(x => x.UserInfoId == currentUserId);
        }
        
        return EmptyQuery(query);
    }

    private IQueryable<Organization> GetOrganizations(DbSet<Organization> query)
    {
        if (_identityService.IsSystem)
        {
            return query;
        }

        if (_identityService.IsAuthenticated)
        {
            return query.Where(organization => organization.Id == _identityService.CurrentUserId!.Value);
        }
        
        return EmptyQuery(query);
    }

    private IQueryable<T> EmptyQuery<T>(IQueryable<T> query) => Enumerable.Empty<T>().AsQueryable();
    
    /// <inheritdoc />
    public Task SaveChanges(CancellationToken cancellationToken)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}