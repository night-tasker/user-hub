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
        UserInfos = new ApplicationDbSet<UserInfo, Guid>(_dbContext, GetUserInfos(_dbContext.Set<UserInfo>()));
    }

    /// <inheritdoc />
    public ApplicationDbSet<UserInfo, Guid> UserInfos { get; }

    private IQueryable<UserInfo> GetUserInfos(DbSet<UserInfo> query)
    {
        var currentUserId = _identityService.CurrentUserId!.Value;
        return query.Where(x => x.Id == currentUserId);
    }
    
    /// <inheritdoc />
    public Task SaveChanges(CancellationToken cancellationToken)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}