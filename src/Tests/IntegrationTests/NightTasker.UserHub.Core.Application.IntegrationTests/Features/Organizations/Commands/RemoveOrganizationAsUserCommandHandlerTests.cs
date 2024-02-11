using Bogus;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NightTasker.Common.Core.Identity.Contracts;
using NightTasker.UserHub.Core.Application.Exceptions.NotFound;
using NightTasker.UserHub.Core.Application.Exceptions.Unauthorized;
using NightTasker.UserHub.Core.Application.Features.Organizations.Commands.RemoveOrganizationAsUser;
using NightTasker.UserHub.Core.Application.Features.Organizations.Services.Contracts;
using NightTasker.UserHub.Core.Application.Features.Organizations.Services.Implementations;
using NightTasker.UserHub.Core.Application.Features.OrganizationUsers.Services.Contracts;
using NightTasker.UserHub.Core.Application.Features.OrganizationUsers.Services.Implementations;
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

namespace NightTasker.UserHub.Core.Application.IntegrationTests.Features.Organizations.Commands;

public class RemoveOrganizationAsUserCommandHandlerTests : ApplicationIntegrationTestsBase
{
    private static readonly Guid UserId = Guid.NewGuid();
    private readonly Faker _faker;
    private readonly CancellationTokenSource _cancellationTokenSource;

    public RemoveOrganizationAsUserCommandHandlerTests(TestNpgSql testNpgSql) : base(testNpgSql) 
    {
        var identityService = Substitute.For<IIdentityService>();
        RegisterService(new ServiceForRegister(typeof(IApplicationDataAccessor), serviceProvider => new ApplicationDataAccessor(
            serviceProvider.GetRequiredService<ApplicationDbContext>()), ServiceLifetime.Scoped));
        RegisterService(new ServiceForRegister(typeof(IUnitOfWork), 
            serviceProvider => new UnitOfWork(serviceProvider.GetRequiredService<IApplicationDataAccessor>()), ServiceLifetime.Scoped));
        RegisterService(new ServiceForRegister(typeof(IOrganizationService), 
            serviceProvider => new OrganizationService(
                serviceProvider.GetRequiredService<IUnitOfWork>()), ServiceLifetime.Scoped));
        
        RegisterService(new ServiceForRegister(typeof(IOrganizationUserService), 
            serviceProvider => new OrganizationUserService(
                serviceProvider.GetRequiredService<IUnitOfWork>()), ServiceLifetime.Scoped));
        
        RegisterService(new ServiceForRegister(typeof(RemoveOrganizationAsUserCommandHandler)));
        
        BuildServiceProvider();
        
        identityService.CurrentUserId.Returns(UserId);
        identityService.IsAuthenticated.Returns(true);

        _faker = new Faker();
        _cancellationTokenSource = new CancellationTokenSource();
        
        PrepareDatabase();
    }
    
    [Fact]
    public async Task Handle_OrganizationExistsAndUserIsAdmin_OrganizationDeleted()
    {
        // Arrange
        var organizationId = Guid.NewGuid();
        await using (var arrangeScope = CreateAsyncScope())
        {
            var dbContext = arrangeScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var organization = SetupRandomOrganization(organizationId);
            await dbContext.Set<Organization>().AddAsync(organization, _cancellationTokenSource.Token);
            await dbContext.Set<User>().AddAsync(SetupUser(UserId), _cancellationTokenSource.Token);
            var organizationUser = SetupOrganizationUser(organization.Id, UserId, OrganizationUserRole.Admin);
            await dbContext.Set<OrganizationUser>().AddAsync(organizationUser, _cancellationTokenSource.Token);
            await dbContext.SaveChangesAsync(_cancellationTokenSource.Token);
        }
        
        var removeOrganizationAsUserCommand = new RemoveOrganizationAsUserCommand(UserId, organizationId);
        
        // Act
        await using (var actScope = CreateAsyncScope())
        {
            var sut = actScope.ServiceProvider.GetRequiredService<RemoveOrganizationAsUserCommandHandler>();
            await sut.Handle(removeOrganizationAsUserCommand, _cancellationTokenSource.Token);
        }
        
        // Assert
        await using (var assertScope = CreateAsyncScope())
        {
            var dbContext = assertScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var removedOrganization = await dbContext.Set<Organization>().SingleOrDefaultAsync(_cancellationTokenSource.Token);
            removedOrganization.Should().BeNull();
        }
    }
    
    [Fact]
    public async Task Handle_OrganizationExistsAndUserIsNotAdmin_ThrowsUserCanNotRemoveOrganizationUnauthorizedException()
    {
        // Arrange
        var organizationId = Guid.NewGuid();
        await using (var arrangeScope = CreateAsyncScope())
        {
            var dbContext = arrangeScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var organization = SetupRandomOrganization(organizationId);
            await dbContext.Set<Organization>().AddAsync(organization, _cancellationTokenSource.Token);
            await dbContext.Set<User>().AddAsync(SetupUser(UserId), _cancellationTokenSource.Token);
            var organizationUser = SetupOrganizationUser(organization.Id, UserId, OrganizationUserRole.Member);
            await dbContext.Set<OrganizationUser>().AddAsync(organizationUser, _cancellationTokenSource.Token);
            await dbContext.SaveChangesAsync(_cancellationTokenSource.Token);
        }

        var removeOrganizationAsUserCommand = new RemoveOrganizationAsUserCommand(UserId, organizationId);
        
        // Act && Assert
        await using (var actScope = CreateAsyncScope())
        {
            var sut = actScope.ServiceProvider.GetRequiredService<RemoveOrganizationAsUserCommandHandler>();
            var act = async () => await sut.Handle(removeOrganizationAsUserCommand, _cancellationTokenSource.Token);
            await act.Should().ThrowAsync<UserCanNotDeleteOrganizationUnauthorizedException>();
        }
        
        // Assert
        await using (var assertScope = CreateAsyncScope())
        {
            var dbContext = assertScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var notRemovedOrganization = await dbContext.Set<Organization>().SingleOrDefaultAsync(_cancellationTokenSource.Token);
            notRemovedOrganization.Should().NotBeNull();
            notRemovedOrganization!.Id.Should().Be(organizationId);
        }
    }
    
    [Fact]
    public async Task Handle_OrganizationDoesNotExist_ThrowsOrganizationNotFoundException()
    {
        // Arrange
        var organizationId = Guid.NewGuid();
        await using (var arrangeScope = CreateAsyncScope())
        {
            var dbContext = arrangeScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await dbContext.Set<User>().AddAsync(SetupUser(UserId), _cancellationTokenSource.Token);
            await dbContext.SaveChangesAsync(_cancellationTokenSource.Token);
        }
        var removeOrganizationAsUserCommand = new RemoveOrganizationAsUserCommand(UserId, organizationId);
        
        // Act && Assert
        await using (var actScope = CreateAsyncScope())
        {
            var sut = actScope.ServiceProvider.GetRequiredService<RemoveOrganizationAsUserCommandHandler>();
            var act = async () => await sut.Handle(removeOrganizationAsUserCommand, _cancellationTokenSource.Token);
            await act.Should().ThrowAsync<OrganizationUserNotFoundException>();
        }
    }
    
    [Fact]
    public async Task HandleOrganization_OrganizationExistsButUserIsNotInOrganization_ThrowsOrganizationNotFoundException()
    {
        // Arrange
        var organizationId = Guid.NewGuid();
        await using (var arrangeScope = CreateAsyncScope())
        {
            var organization = SetupRandomOrganization(organizationId);
            var dbContext = arrangeScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await dbContext.Set<User>().AddAsync(SetupUser(UserId), _cancellationTokenSource.Token);
            await dbContext.Set<Organization>().AddAsync(organization, _cancellationTokenSource.Token);
            await dbContext.SaveChangesAsync(_cancellationTokenSource.Token);
        }

        var removeOrganizationAsUserCommand = new RemoveOrganizationAsUserCommand(UserId, organizationId);
        
        // Act && Assert
        await using (var actScope = CreateAsyncScope())
        {
            var sut = actScope.ServiceProvider.GetRequiredService<RemoveOrganizationAsUserCommandHandler>();
            var act = async () => await sut.Handle(removeOrganizationAsUserCommand, _cancellationTokenSource.Token);
            await act.Should().ThrowAsync<OrganizationUserNotFoundException>();
        }
    }

    private static User SetupUser(Guid userId)
    {
        return User.CreateInstance(userId);
    }

    private Organization SetupRandomOrganization(Guid id)
    {
        return Organization.CreateInstance(id, _faker.Random.AlphaNumeric(8), _faker.Random.AlphaNumeric(32));
    }

    private static OrganizationUser SetupOrganizationUser(Guid organizationId, Guid userId, OrganizationUserRole role)
    {
        return OrganizationUser.CreateInstance(organizationId, userId, role);
    }
}