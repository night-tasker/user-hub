using Microsoft.EntityFrameworkCore;
using NightTasker.Common.Core.Identity.Contracts;
using NightTasker.Common.Core.Persistence;
using NightTasker.UserHub.Core.Domain.Entities;
using NightTasker.UserHub.Infrastructure.Persistence;
using NightTasker.UserHub.Infrastructure.Persistence.Contracts;

namespace NightTasker.UserHub.Presentation.WebApi.Implementations;

/// <inheritdoc /> 
public class ApplicationDbAccessor(ApplicationDbContext dbContext, IIdentityService identityService) : IApplicationDbAccessor
{
    private readonly ApplicationDbContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

    private readonly IIdentityService _identityService =
        identityService ?? throw new ArgumentNullException(nameof(identityService));

    /// <inheritdoc />
    public ApplicationDbSet<UserInfo, Guid> UserInfos => new(_dbContext, GetUserInfos(_dbContext.Set<UserInfo>()));

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