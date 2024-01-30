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
using Xunit;

namespace NightTasker.UserHub.Core.Application.IntegrationTests.Features.Organization.Queries;

public class GetOrganizationByIdQueryHandlerTests : ApplicationIntegrationTestsBase
{
    private readonly ISender _sender;
    private static readonly Guid UserId = Guid.NewGuid();
    private readonly Faker _faker; 
    
    public GetOrganizationByIdQueryHandlerTests()
    {
        var identityService = Substitute.For<IIdentityService>();
        RegisterService(new ServiceForRegister(typeof(IIdentityService), _ => identityService, ServiceLifetime.Scoped));
        RegisterService(new ServiceForRegister(typeof(IApplicationDbAccessor), serviceProvider => new ApplicationDbAccessor(
            serviceProvider.GetRequiredService<ApplicationDbContext>(), serviceProvider.GetRequiredService<IIdentityService>()), ServiceLifetime.Scoped));
        RegisterService(new ServiceForRegister(typeof(IUnitOfWork), 
            serviceProvider => new UnitOfWork(serviceProvider.GetRequiredService<IApplicationDbAccessor>()), ServiceLifetime.Scoped));
        
        BuildServiceProvider();
        
        identityService.CurrentUserId.Returns(UserId);
        identityService.IsAuthenticated.Returns(true);

        var dbContext = GetService<ApplicationDbContext>();
        dbContext.Database.Migrate();
        _sender = GetService<ISender>();
        _faker = new Faker();
    }

    [Theory]
    [InlineData(OrganizationUserRole.Admin)]
    [InlineData(OrganizationUserRole.Member)]
    public async Task Handle_UserWithRoleInOrganization_ReturnsRightUsersCount(OrganizationUserRole currentUserRoleInOrganization)
    {
        // Arrange
        var dbContext = GetService<ApplicationDbContext>();
        var users = new List<UserInfo>
        {
            SetupUserInfo(UserId), SetupUserInfo(Guid.NewGuid()), SetupUserInfo(Guid.NewGuid()), SetupUserInfo(Guid.NewGuid())
        };
        
        dbContext.Set<UserInfo>().AddRange(users);
        
        var organization = SetupOrganization();
        dbContext.Set<Domain.Entities.Organization>().Add(organization);
        
        var organizationUsers = new List<OrganizationUser>
        {
            SetupOrganizationUser(organization.Id, users[0].Id, currentUserRoleInOrganization),
            SetupOrganizationUser(organization.Id, users[1].Id, OrganizationUserRole.Member),
            SetupOrganizationUser(organization.Id, users[2].Id, OrganizationUserRole.Admin),
            SetupOrganizationUser(organization.Id, users[3].Id, OrganizationUserRole.Member)
        };
        dbContext.Set<OrganizationUser>().AddRange(organizationUsers);
        
        await dbContext.SaveChangesAsync();
        
        // Act
        var query = new GetOrganizationByIdQuery(organization.Id);
        var result = await _sender.Send(query);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(organization.Id);
        result.Name.Should().Be(organization.Name);
        result.Description.Should().Be(organization.Description);
        result.UsersCount.Should().Be(4);
    }

    [Fact]
    public async Task Handle_OrganizationDoesNotExist_OrganizationNotFoundException()
    {
        // Arrange
        var notExistingOrganizationId = Guid.NewGuid();

        // Act
        var query = new GetOrganizationByIdQuery(notExistingOrganizationId);
        Func<Task> act = async () => await _sender.Send(query);
        
        // Assert
        await act.Should().ThrowAsync<OrganizationNotFoundException>();
    }
    
    [Fact]
    public async Task Handle_OrganizationExistsButUserIsNotOrganizationUser_OrganizationNotFoundException()
    {
        // Arrange
        var dbContext = GetService<ApplicationDbContext>();
        dbContext.Set<UserInfo>().Add(SetupUserInfo(UserId));
        var organization = SetupOrganization();
        dbContext.Set<Domain.Entities.Organization>().Add(organization);
        
        await dbContext.SaveChangesAsync();
        
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