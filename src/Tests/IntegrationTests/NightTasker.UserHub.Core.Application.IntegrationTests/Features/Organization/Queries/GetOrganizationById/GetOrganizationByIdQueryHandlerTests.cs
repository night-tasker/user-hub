using Bogus;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NightTasker.Common.Core.Identity.Contracts;
using NightTasker.UserHub.Core.Application.ApplicationContracts.Repository;
using NightTasker.UserHub.Core.Application.Exceptions.NotFound;
using NightTasker.UserHub.Core.Application.Features.Organizations.Queries.GetOrganizationById;
using NightTasker.UserHub.Core.Domain.Entities;
using NightTasker.UserHub.Core.Domain.Enums;
using NightTasker.UserHub.Infrastructure.Persistence;
using NightTasker.UserHub.Infrastructure.Persistence.Contracts;
using NightTasker.UserHub.Infrastructure.Persistence.Repository.Common;
using NightTasker.UserHub.IntegrationTests.Framework;
using NightTasker.UserHub.Presentation.WebApi.Implementations;
using NSubstitute;

namespace NightTasker.UserHub.Core.Application.IntegrationTests.Features.Organization.Queries.GetOrganizationById;

public class GetOrganizationByIdQueryHandlerTests : ApplicationIntegrationTestsBase
{
    private ISender _sender = null!;
    private static readonly Guid UserId = Guid.NewGuid();
    private IIdentityService _identityService = null!;
    private Faker _faker = null!; 

    [SetUp]
    public async Task Setup()
    {
        _identityService = Substitute.For<IIdentityService>();
        RegisterService(new ServiceForRegister(typeof(IIdentityService), _ => _identityService, ServiceLifetime.Scoped));
        RegisterService(new ServiceForRegister(typeof(IApplicationDbAccessor), serviceProvider => new ApplicationDbAccessor(
            serviceProvider.GetRequiredService<ApplicationDbContext>(), serviceProvider.GetRequiredService<IIdentityService>()), ServiceLifetime.Scoped));
        RegisterService(new ServiceForRegister(typeof(IUnitOfWork), 
            serviceProvider => new UnitOfWork(serviceProvider.GetRequiredService<IApplicationDbAccessor>()), ServiceLifetime.Scoped));
        
        BuildServiceProvider();

        var dbContext = GetService<ApplicationDbContext>();
        await dbContext.Database.MigrateAsync();
        _sender = GetService<ISender>();
        _faker = new Faker();
    }

    [Test]
    public async Task Handle_OrganizationWithOneUser_ReturnsRightUsersCount()
    {
        // Arrange
        var dbContext = GetService<ApplicationDbContext>();
        dbContext.Set<UserInfo>().Add(SetupUserInfo(UserId));
        var organization = SetupOrganization();
        dbContext.Set<Domain.Entities.Organization>().Add(organization);
        dbContext.Set<OrganizationUser>().Add(SetupOrganizationUser(organization.Id, UserId, OrganizationUserRole.Admin));
        
        await dbContext.SaveChangesAsync();
        
        _identityService.CurrentUserId.Returns(UserId);
        _identityService.IsAuthenticated.Returns(true);
        
        // Act
        var query = new GetOrganizationByIdQuery(organization.Id);
        var result = await _sender.Send(query);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(organization.Id);
        result.Name.Should().Be(organization.Name);
        result.Description.Should().Be(organization.Description);
        result.UsersCount.Should().Be(1);
    }

    [Test]
    public async Task Handle_OrganizationDoesNotExist_OrganizationNotFoundException()
    {
        // Arrange
        var notExistingOrganizationId = Guid.NewGuid();
        
        _identityService.CurrentUserId.Returns(UserId);
        _identityService.IsAuthenticated.Returns(true);
        
        // Act
        var query = new GetOrganizationByIdQuery(notExistingOrganizationId);
        Func<Task> act = async () => await _sender.Send(query);
        
        // Assert
        await act.Should().ThrowAsync<OrganizationNotFoundException>();
    }
    
    [Test]
    public async Task Handle_OrganizationExistsButUserIsNotOrganizationUser_OrganizationNotFoundException()
    {
        // Arrange
        var dbContext = GetService<ApplicationDbContext>();
        dbContext.Set<UserInfo>().Add(SetupUserInfo(UserId));
        var organization = SetupOrganization();
        dbContext.Set<Domain.Entities.Organization>().Add(organization);
        
        await dbContext.SaveChangesAsync();
        
        _identityService.CurrentUserId.Returns(UserId);
        _identityService.IsAuthenticated.Returns(true);
        
        // Act
        var query = new GetOrganizationByIdQuery(organization.Id);
        Func<Task> act = async () => await _sender.Send(query);
        
        // Assert
        await act.Should().ThrowAsync<OrganizationNotFoundException>();
    }

    private OrganizationUser SetupOrganizationUser(Guid organizationId, Guid userId, OrganizationUserRole role)
    {
        return new OrganizationUser
        {
            UserId = userId,
            OrganizationId = organizationId,
            Role = role
        };
    }
    
    private Domain.Entities.Organization SetupOrganization()
    {
        return new Domain.Entities.Organization
        {
            Id = Guid.NewGuid(), 
            Name = _faker.Random.AlphaNumeric(8), 
            Description = _faker.Random.AlphaNumeric(32)
        };
    }

    private UserInfo SetupUserInfo(Guid userId)
    {
        return new UserInfo
        {
            Id = userId
        };
    }
}