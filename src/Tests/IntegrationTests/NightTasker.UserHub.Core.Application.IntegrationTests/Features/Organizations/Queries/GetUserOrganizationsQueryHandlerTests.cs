﻿using Bogus;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NightTasker.Common.Core.Identity.Contracts;
using NightTasker.UserHub.Core.Application.Features.Organizations.Queries.GetUserOrganizations;
using NightTasker.UserHub.Core.Domain.Entities;
using NightTasker.UserHub.Core.Domain.Enums;
using NightTasker.UserHub.Core.Domain.Repositories;
using NightTasker.UserHub.Infrastructure.Persistence;
using NightTasker.UserHub.Infrastructure.Persistence.Contracts;
using NightTasker.UserHub.Infrastructure.Persistence.Repository.Common;
using NightTasker.UserHub.IntegrationTests.Framework;
using NightTasker.UserHub.Presentation.WebApi.Implementations;
using NSubstitute;
using Xunit;

namespace NightTasker.UserHub.Core.Application.IntegrationTests.Features.Organizations.Queries;

public class GetUserOrganizationsQueryHandlerTests : ApplicationIntegrationTestsBase
{
    private static readonly Guid UserId = Guid.NewGuid();
    private readonly Faker _faker;
    private readonly CancellationTokenSource _cancellationTokenSource;

    public GetUserOrganizationsQueryHandlerTests()
    {
        var identityService = Substitute.For<IIdentityService>();
        RegisterService(new ServiceForRegister(typeof(IApplicationDbAccessor), serviceProvider => new ApplicationDbAccessor(
            serviceProvider.GetRequiredService<ApplicationDbContext>()), ServiceLifetime.Scoped));
        RegisterService(new ServiceForRegister(typeof(IUnitOfWork), 
            serviceProvider => new UnitOfWork(serviceProvider.GetRequiredService<IApplicationDbAccessor>()), ServiceLifetime.Scoped));
        RegisterService(new ServiceForRegister(typeof(GetUserOrganizationsQueryHandler)));
        BuildServiceProvider();
        
        identityService.CurrentUserId.Returns(UserId);
        identityService.IsAuthenticated.Returns(true);

        var dbContext = GetService<ApplicationDbContext>();
        dbContext.Database.Migrate();
        
        _faker = new Faker();
        _cancellationTokenSource = new CancellationTokenSource();
    }
    
    [Theory]
    [InlineData(OrganizationUserRole.Admin)]
    [InlineData(OrganizationUserRole.Member)]
    public async Task Handle_UserHasRoleInOrganizations_ReturnsOrganizations(OrganizationUserRole role)
    {
        // Arrange
        var dbContext = GetService<ApplicationDbContext>();
        var user = SetupUserInfo(UserId);
        await dbContext.Set<UserInfo>().AddAsync(user);

        var organizations = new List<Organization>
        {
            SetupOrganization(),
            SetupOrganization(),
            SetupOrganization()
        };
        await dbContext.Set<Organization>().AddRangeAsync(organizations);

        var organizationUsers = new List<OrganizationUser>
        {
            SetupOrganizationUser(organizations[0].Id, user.Id, role),
            SetupOrganizationUser(organizations[1].Id, user.Id, role),
            SetupOrganizationUser(organizations[2].Id, user.Id, role)
        };
        await dbContext.Set<OrganizationUser>().AddRangeAsync(organizationUsers);
        await dbContext.SaveChangesAsync();
        
        var query = new GetUserOrganizationsQuery(UserId);
        var sut = GetService<GetUserOrganizationsQueryHandler>();
        
        // Act
        var result = await sut.Handle(query, _cancellationTokenSource.Token);
        
        // Assert
        Assert.Equal(organizations.Count, result.Count);
        result.Select(x => x.Id).Should()
            .BeEquivalentTo(organizations.Select(x => x.Id));
    }
    
    [Fact]
    public async Task Handle_UserHasNotOrganizationsButOtherUsersHave_ReturnsEmptyCollection()
    {
        // Arrange
        var dbContext = GetService<ApplicationDbContext>();
        var user = SetupUserInfo(UserId);
        await dbContext.Set<UserInfo>().AddAsync(user);
        
        var otherUsers = new List<UserInfo>
        {
            SetupUserInfo(Guid.NewGuid()),
            SetupUserInfo(Guid.NewGuid()),
            SetupUserInfo(Guid.NewGuid())
        };
        await dbContext.Set<UserInfo>().AddRangeAsync(otherUsers);

        var organizations = new List<Organization>
        {
            SetupOrganization(),
            SetupOrganization(),
            SetupOrganization()
        };
        await dbContext.Set<Organization>().AddRangeAsync(organizations);

        var organizationUsers = new List<OrganizationUser>
        {
            SetupOrganizationUser(organizations[0].Id, otherUsers[0].Id, OrganizationUserRole.Admin),
            SetupOrganizationUser(organizations[1].Id, otherUsers[1].Id, OrganizationUserRole.Member),
            SetupOrganizationUser(organizations[1].Id, otherUsers[2].Id, OrganizationUserRole.Member),
            SetupOrganizationUser(organizations[2].Id, otherUsers[1].Id, OrganizationUserRole.Member),
            SetupOrganizationUser(organizations[2].Id, otherUsers[2].Id, OrganizationUserRole.Admin)
        };
        await dbContext.Set<OrganizationUser>().AddRangeAsync(organizationUsers);
        await dbContext.SaveChangesAsync();
        
        var query = new GetUserOrganizationsQuery(UserId);
        var sut = GetService<GetUserOrganizationsQueryHandler>();
        
        // Act
        var result = await sut.Handle(query, _cancellationTokenSource.Token);
        
        // Assert
        Assert.Equal(0, result.Count);
    }

    
    private static OrganizationUser SetupOrganizationUser(Guid organizationId, Guid userId, OrganizationUserRole role)
    {
        return new OrganizationUser
        {
            UserId = userId,
            OrganizationId = organizationId,
            Role = role
        };
    }
    
    private Organization SetupOrganization()
    {
        return new Organization
        {
            Id = Guid.NewGuid(), 
            Name = _faker.Random.AlphaNumeric(8), 
            Description = _faker.Random.AlphaNumeric(32)
        };
    }

    private static UserInfo SetupUserInfo(Guid userId)
    {
        return new UserInfo
        {
            Id = userId
        };
    }
}