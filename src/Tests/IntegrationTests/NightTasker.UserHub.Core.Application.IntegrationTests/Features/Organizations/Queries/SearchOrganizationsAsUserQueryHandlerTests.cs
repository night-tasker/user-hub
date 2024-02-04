using Bogus;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NightTasker.Common.Core.Identity.Contracts;
using NightTasker.UserHub.Core.Application.Features.Organizations.Models.Search;
using NightTasker.UserHub.Core.Application.Features.Organizations.Queries.SearchOrganizationsAsUser;
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

public class SearchOrganizationsAsUserQueryHandlerTests : ApplicationIntegrationTestsBase
{
    private static readonly Guid UserId = Guid.NewGuid();
    private readonly Faker _faker;
    private readonly CancellationTokenSource _cancellationTokenSource;

    public SearchOrganizationsAsUserQueryHandlerTests()
    {
        var identityService = Substitute.For<IIdentityService>();
        RegisterService(new ServiceForRegister(typeof(IApplicationDataAccessor), serviceProvider => new ApplicationDataAccessor(
            serviceProvider.GetRequiredService<ApplicationDbContext>()), ServiceLifetime.Scoped));
        RegisterService(new ServiceForRegister(typeof(IUnitOfWork), 
            serviceProvider => new UnitOfWork(serviceProvider.GetRequiredService<IApplicationDataAccessor>()), ServiceLifetime.Scoped));
        RegisterService(new ServiceForRegister(typeof(SearchOrganizationsAsUserQueryHandler)));
        BuildServiceProvider();
        
        identityService.CurrentUserId.Returns(UserId);
        identityService.IsAuthenticated.Returns(true);

        var dbContext = GetService<ApplicationDbContext>();
        dbContext.Database.Migrate();
        
        _faker = new Faker();
        _cancellationTokenSource = new CancellationTokenSource();
    }

    [Fact]
    public async Task Handle_UserHasNotOrganizations_ReturnsEmpty()
    {
        // Arrange
        await using (var arrangeScope = CreateAsyncScope())
        {
            var organizations = new [] { SetupOrganization(), SetupOrganization(), SetupOrganization() };
            var user = SetupUser(UserId);
            var dbContext = arrangeScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await dbContext.Set<User>().AddAsync(user, _cancellationTokenSource.Token);
            await dbContext.Set<Organization>().AddRangeAsync(organizations, _cancellationTokenSource.Token);
            await dbContext.SaveChangesAsync(_cancellationTokenSource.Token);
        }
        var query = new SearchOrganizationsAsUserQuery(UserId, new OrganizationsSearchCriteria());
        
        // Act
        await using var actScope = CreateAsyncScope();
        var handler = actScope.ServiceProvider.GetRequiredService<SearchOrganizationsAsUserQueryHandler>();
        var result = await handler.Handle(query, _cancellationTokenSource.Token);
        
        // Assert
        result.Items.Should().BeEmpty();
        result.Page.Should().Be(1);
        result.Take.Should().Be(0);
        result.TotalCount.Should().Be(0);
    }
    
    [Fact]
    public async Task Handle_UserHasOrganizations_ReturnsOrganizations()
    {
        var organizations = new [] { SetupOrganization(), SetupOrganization(), SetupOrganization() };
        // Arrange
        await using (var arrangeScope = CreateAsyncScope())
        {
            var user = SetupUser(UserId);
            var organizationUsers = organizations.Select(
                x => SetupOrganizationUser(x.Id, user.Id, OrganizationUserRole.Admin));
            
            var dbContext = arrangeScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await dbContext.Set<User>().AddAsync(user, _cancellationTokenSource.Token);
            await dbContext.Set<Organization>().AddRangeAsync(organizations, _cancellationTokenSource.Token);
            await dbContext.Set<OrganizationUser>().AddRangeAsync(organizationUsers, _cancellationTokenSource.Token);
            await dbContext.SaveChangesAsync(_cancellationTokenSource.Token);
        }
        var query = new SearchOrganizationsAsUserQuery(UserId, new OrganizationsSearchCriteria());
        
        // Act
        await using var actScope = CreateAsyncScope();
        var handler = actScope.ServiceProvider.GetRequiredService<SearchOrganizationsAsUserQueryHandler>();
        var result = await handler.Handle(query, _cancellationTokenSource.Token);
        
        // Assert
        result.Items.Should().NotBeEmpty();
        organizations.Length.Should().Be(result.Items.Count);
        result.Page.Should().Be(1);
        result.Take.Should().Be(organizations.Length);
        result.TotalCount.Should().Be(organizations.Length);
        result.Items.Select(x => x.Id).Should().BeEquivalentTo(organizations.Select(x => x.Id));
    }
    
    [Fact]
    public async Task Handle_UserHasNotAllOrganizations_ReturnsOrganizations()
    {
        var organizations = new [] { SetupOrganization(), SetupOrganization(), SetupOrganization(), SetupOrganization() };
        var currentUserOrganizations = organizations.Take(2).ToList();
        // Arrange
        await using (var arrangeScope = CreateAsyncScope())
        {
            var user = SetupUser(UserId);
            var organizationUsers = currentUserOrganizations.Select(
                x => SetupOrganizationUser(x.Id, user.Id, OrganizationUserRole.Admin));
            
            var dbContext = arrangeScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await dbContext.Set<User>().AddAsync(user, _cancellationTokenSource.Token);
            await dbContext.Set<Organization>().AddRangeAsync(organizations, _cancellationTokenSource.Token);
            await dbContext.Set<OrganizationUser>().AddRangeAsync(organizationUsers, _cancellationTokenSource.Token);
            await dbContext.SaveChangesAsync(_cancellationTokenSource.Token);
        }
        var query = new SearchOrganizationsAsUserQuery(UserId, new OrganizationsSearchCriteria());
        
        // Act
        await using var actScope = CreateAsyncScope();
        var handler = actScope.ServiceProvider.GetRequiredService<SearchOrganizationsAsUserQueryHandler>();
        var result = await handler.Handle(query, _cancellationTokenSource.Token);
        
        // Assert
        result.Items.Should().NotBeEmpty();
        currentUserOrganizations.Count.Should().Be(result.Items.Count);
        result.Page.Should().Be(1);
        result.Take.Should().Be(currentUserOrganizations.Count);
        result.TotalCount.Should().Be(currentUserOrganizations.Count);
        result.Items.Select(x => x.Id).Should().BeEquivalentTo(currentUserOrganizations.Select(x => x.Id));
    }
    
    private OrganizationUser SetupOrganizationUser(Guid organizationId, Guid userId, OrganizationUserRole role)
    {
        return new OrganizationUser
        {
            OrganizationId = organizationId,
            UserId = userId,
            Role = role
        };
    }

    private User SetupUser(Guid userId)
    {
        return new User
        {
            Id = userId
        };
    }
    
    private Organization SetupOrganization()
    {
        return new Organization
        {
            Id = Guid.NewGuid(),
            Name = _faker.Company.CompanyName(),
            Description = _faker.Company.CatchPhrase()
        };
    }
}