﻿using Microsoft.EntityFrameworkCore;
using NightTasker.Common.Core.Persistence;
using NightTasker.Common.Core.Persistence.Repository;
using NightTasker.UserHub.Core.Domain.Entities;
using NightTasker.UserHub.Core.Domain.Repositories;

namespace NightTasker.UserHub.Infrastructure.Persistence.Repository;

public class UserRepository
    (ApplicationDbSet<User, Guid> dbSet) : BaseRepository<User, Guid>(dbSet), IUserRepository
{
    public Task<User?> TryGetById(
        Guid id, 
        bool trackChanges, 
        CancellationToken cancellationToken)
    {
        var entities = Entities;
        if (!trackChanges)
        {
            entities = entities.AsNoTracking();
        }
        
        return entities
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task<bool> CheckExistsById(Guid id, CancellationToken cancellationToken)
    {
        return Entities
            .AnyAsync(x => x.Id == id, cancellationToken);
    }
}