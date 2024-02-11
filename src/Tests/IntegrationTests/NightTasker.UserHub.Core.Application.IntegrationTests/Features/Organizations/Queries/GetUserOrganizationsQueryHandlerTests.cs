using Bogus;
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

    public GetUserOrganizationsQueryHandlerTests(TestNpgSql testNpgSql) : base(testNpgSql)
    {
        var identityService = Substitute.For<IIdentityService>();
        RegisterService(new ServiceForRegister(typeof(IApplicationDataAccessor), serviceProvider => new ApplicationDataAccessor(
            serviceProvider.GetRequiredService<ApplicationDbContext>()), ServiceLifetime.Scoped));
        RegisterService(new ServiceForRegister(typeof(IUnitOfWork), 
            serviceProvider => new UnitOfWork(serviceProvider.GetRequiredService<IApplicationDataAccessor>()), ServiceLifetime.Scoped));
        RegisterService(new ServiceForRegister(typeof(GetUserOrganizationsQueryHandler)));
        BuildServiceProvider();
        
        identityService.CurrentUserId.Returns(UserId);
        identityService.IsAuthenticated.Returns(true);

        PrepareDatabase();
        
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
        var user = SetupUser(UserId);
        await dbContext.Set<User>().AddAsync(user);

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
        var user = SetupUser(UserId);
        await dbContext.Set<User>().AddAsync(user);
        
        var otherUsers = new List<User>
        {
            SetupUser(Guid.NewGuid()),
            SetupUser(Guid.NewGuid()),
            SetupUser(Guid.NewGuid())
        };
        await dbContext.Set<User>().AddRangeAsync(otherUsers);

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
        return OrganizationUser.CreateInstance(organizationId, userId, role);
    }
    
    private Organization SetupOrganization()
    {
        return Organization.CreateInstance(_faker.Random.AlphaNumeric(8), _faker.Random.AlphaNumeric(32));
    }

    private static User SetupUser(Guid userId)
    {
        return User.CreateInstance(userId);
    }
}