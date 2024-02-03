using Bogus;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NightTasker.Common.Core.Identity.Contracts;
using NightTasker.UserHub.Core.Application.Exceptions.NotFound;
using NightTasker.UserHub.Core.Application.Exceptions.Unauthorized;
using NightTasker.UserHub.Core.Application.Features.Organizations.Commands.UpdateOrganizationAsUser;
using NightTasker.UserHub.Core.Application.Features.Organizations.Models;
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

public class UpdateOrganizationAsUserCommandHandlerTests : ApplicationIntegrationTestsBase
{
    private static readonly Guid UserId = Guid.NewGuid();
    private readonly Faker _faker;
    private readonly CancellationTokenSource _cancellationTokenSource;

    public UpdateOrganizationAsUserCommandHandlerTests()
    {
        var identityService = Substitute.For<IIdentityService>();
        RegisterService(new ServiceForRegister(typeof(IApplicationDbAccessor), serviceProvider => new ApplicationDbAccessor(
            serviceProvider.GetRequiredService<ApplicationDbContext>()), ServiceLifetime.Scoped));
        RegisterService(new ServiceForRegister(typeof(IUnitOfWork), 
            serviceProvider => new UnitOfWork(serviceProvider.GetRequiredService<IApplicationDbAccessor>()), ServiceLifetime.Scoped));
        RegisterService(new ServiceForRegister(typeof(IOrganizationService), 
            serviceProvider => new OrganizationService(
                serviceProvider.GetRequiredService<IUnitOfWork>()), ServiceLifetime.Scoped));
        
        RegisterService(new ServiceForRegister(typeof(IOrganizationUserService), 
            serviceProvider => new OrganizationUserService(
                serviceProvider.GetRequiredService<IUnitOfWork>()), ServiceLifetime.Scoped));
        
        RegisterService(new ServiceForRegister(typeof(UpdateOrganizationAsUserCommandHandler)));
        
        BuildServiceProvider();
        
        identityService.CurrentUserId.Returns(UserId);
        identityService.IsAuthenticated.Returns(true);

        _faker = new Faker();
        _cancellationTokenSource = new CancellationTokenSource();
        var scope = CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        dbContext.Database.Migrate();
    }
    
    [Fact]
    public async Task UpdateOrganization_OrganizationExistsAndUserIsAdmin_OrganizationUpdated()
    {
        // Arrange
        var organizationId = Guid.NewGuid();
        await using (var arrangeScope = CreateAsyncScope())
        {
            var dbContext = arrangeScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var organization = SetupRandomOrganization(organizationId);
            await dbContext.Set<Organization>().AddAsync(organization, _cancellationTokenSource.Token);
            await dbContext.Set<UserInfo>().AddAsync(SetupUserInfo(UserId), _cancellationTokenSource.Token);
            var organizationUser = SetupOrganizationUser(organization.Id, UserId, OrganizationUserRole.Admin);
            await dbContext.Set<OrganizationUser>().AddAsync(organizationUser, _cancellationTokenSource.Token);
            await dbContext.SaveChangesAsync(_cancellationTokenSource.Token);
        }
        
        var updateOrganizationDto = new UpdateOrganizationDto(
            _faker.Random.AlphaNumeric(8), _faker.Random.AlphaNumeric(8));
        var updateOrganizationAsUserCommand = new UpdateOrganizationAsUserCommand(
            UserId, organizationId, updateOrganizationDto);
        
        // Act
        await using (var actScope = CreateAsyncScope())
        {
            var sut = actScope.ServiceProvider.GetRequiredService<UpdateOrganizationAsUserCommandHandler>();
            await sut.Handle(updateOrganizationAsUserCommand, _cancellationTokenSource.Token);
        }
        
        // Assert
        await using (var assertScope = CreateAsyncScope())
        {
            var dbContext = assertScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var updatedOrganization = await dbContext.Set<Organization>().SingleOrDefaultAsync(_cancellationTokenSource.Token);
        
            updatedOrganization.Should().NotBeNull();
            updatedOrganization!.Id.Should().Be(organizationId);
            updatedOrganization.Name.Should().Be(updateOrganizationDto.Name);
            updatedOrganization.Description.Should().Be(updateOrganizationDto.Description);
        }
    }
    
    [Fact]
    public async Task UpdateOrganization_OrganizationExistsAndUserIsNotAdmin_ThrowsUserCanNotUpdateOrganizationUnauthorizedException()
    {
        // Arrange
        var organizationId = Guid.NewGuid();
        await using (var arrangeScope = CreateAsyncScope())
        {
            var dbContext = arrangeScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var organization = SetupRandomOrganization(organizationId);
            await dbContext.Set<Organization>().AddAsync(organization, _cancellationTokenSource.Token);
            await dbContext.Set<UserInfo>().AddAsync(SetupUserInfo(UserId), _cancellationTokenSource.Token);
            var organizationUser = SetupOrganizationUser(organization.Id, UserId, OrganizationUserRole.Member);
            await dbContext.Set<OrganizationUser>().AddAsync(organizationUser, _cancellationTokenSource.Token);
            await dbContext.SaveChangesAsync(_cancellationTokenSource.Token);
        }
        
        var updateOrganizationDto = new UpdateOrganizationDto(
            _faker.Random.AlphaNumeric(8), _faker.Random.AlphaNumeric(8));
        var updateOrganizationAsUserCommand = new UpdateOrganizationAsUserCommand(
            UserId, organizationId, updateOrganizationDto);
        
        // Act && Assert
        await using (var actScope = CreateAsyncScope())
        {
            var sut = actScope.ServiceProvider.GetRequiredService<UpdateOrganizationAsUserCommandHandler>();
            var act = async () => await sut.Handle(updateOrganizationAsUserCommand, _cancellationTokenSource.Token);
            await act.Should().ThrowAsync<UserCanNotUpdateOrganizationUnauthorizedException>();
        }
    }
    
    [Fact]
    public async Task UpdateOrganization_OrganizationDoesNotExist_ThrowsOrganizationNotFoundException()
    {
        // Arrange
        var organizationId = Guid.NewGuid();
        await using (var arrangeScope = CreateAsyncScope())
        {
            var dbContext = arrangeScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await dbContext.Set<UserInfo>().AddAsync(SetupUserInfo(UserId), _cancellationTokenSource.Token);
            await dbContext.SaveChangesAsync(_cancellationTokenSource.Token);
        }
        
        var updateOrganizationDto = new UpdateOrganizationDto(
            _faker.Random.AlphaNumeric(8), _faker.Random.AlphaNumeric(8));
        var updateOrganizationAsUserCommand = new UpdateOrganizationAsUserCommand(
            UserId, organizationId, updateOrganizationDto);
        
        // Act && Assert
        await using (var actScope = CreateAsyncScope())
        {
            var sut = actScope.ServiceProvider.GetRequiredService<UpdateOrganizationAsUserCommandHandler>();
            var act = async () => await sut.Handle(updateOrganizationAsUserCommand, _cancellationTokenSource.Token);
            await act.Should().ThrowAsync<OrganizationUserNotFoundException>();
        }
    }
    
    [Fact]
    public async Task UpdateOrganization_OrganizationExistsButUserIsNotInOrganization_ThrowsOrganizationNotFoundException()
    {
        // Arrange
        var organizationId = Guid.NewGuid();
        await using (var arrangeScope = CreateAsyncScope())
        {
            var organization = SetupRandomOrganization(organizationId);
            var dbContext = arrangeScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await dbContext.Set<UserInfo>().AddAsync(SetupUserInfo(UserId), _cancellationTokenSource.Token);
            await dbContext.Set<Organization>().AddAsync(organization, _cancellationTokenSource.Token);
            await dbContext.SaveChangesAsync(_cancellationTokenSource.Token);
        }
        
        var updateOrganizationDto = new UpdateOrganizationDto(
            _faker.Random.AlphaNumeric(8), _faker.Random.AlphaNumeric(8));
        var updateOrganizationAsUserCommand = new UpdateOrganizationAsUserCommand(
            UserId, organizationId, updateOrganizationDto);
        
        // Act && Assert
        await using (var actScope = CreateAsyncScope())
        {
            var sut = actScope.ServiceProvider.GetRequiredService<UpdateOrganizationAsUserCommandHandler>();
            var act = async () => await sut.Handle(updateOrganizationAsUserCommand, _cancellationTokenSource.Token);
            await act.Should().ThrowAsync<OrganizationUserNotFoundException>();
        }
    }

    private static UserInfo SetupUserInfo(Guid userId)
    {
        return new UserInfo
        {
            Id = userId
        };
    }

    private Organization SetupRandomOrganization(Guid id)
    {
        return new Organization
        {
            Id = id,
            Name = _faker.Random.AlphaNumeric(8),
            Description = _faker.Random.AlphaNumeric(32),
        };
    }

    private static OrganizationUser SetupOrganizationUser(Guid organizationId, Guid userId, OrganizationUserRole role)
    {
        return new OrganizationUser
        {
            OrganizationId = organizationId,
            UserId = userId,
            Role = role
        };
    }
}