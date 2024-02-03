﻿using NightTasker.UserHub.Core.Domain.Repositories;
using NightTasker.UserHub.Infrastructure.Persistence.Contracts;

namespace NightTasker.UserHub.Infrastructure.Persistence.Repository.Common;

public class UnitOfWork(IApplicationDbAccessor applicationDbAccessor) : IUnitOfWork
{
    public IUserInfoRepository UserInfoRepository { get; } = new UserInfoRepository(applicationDbAccessor.UserInfos);
    
    public IUserImageRepository UserImageRepository { get; } = new UserImageRepository(applicationDbAccessor.UserImages);
    
    public IOrganizationRepository OrganizationRepository { get; } = new OrganizationRepository(applicationDbAccessor.Organizations);

    public IOrganizationUserRepository OrganizationUserRepository { get; } =
        new OrganizationUserRepository(applicationDbAccessor.OrganizationUsers);

    public Task SaveChanges(CancellationToken cancellationToken)
    {
        return applicationDbAccessor.SaveChanges(cancellationToken);
    }
}