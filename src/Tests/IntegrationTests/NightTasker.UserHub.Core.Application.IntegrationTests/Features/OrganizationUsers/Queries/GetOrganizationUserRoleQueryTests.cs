using Bogus;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NightTasker.UserHub.Core.Application.ApplicationContracts.Repository;
using NightTasker.UserHub.Core.Application.Exceptions.NotFound;
using NightTasker.UserHub.Core.Application.Features.OrganizationUsers.Queries.GetOrganizationUserRole;
using NightTasker.UserHub.Core.Domain.Entities;
using NightTasker.UserHub.Core.Domain.Enums;
using NightTasker.UserHub.Infrastructure.Persistence;
using NightTasker.UserHub.Infrastructure.Persistence.Contracts;
using NightTasker.UserHub.Infrastructure.Persistence.Repository.Common;
using NightTasker.UserHub.IntegrationTests.Framework;
using NightTasker.UserHub.Presentation.WebApi.Implementations;
using Xunit;

namespace NightTasker.UserHub.Core.Application.IntegrationTests.Features.OrganizationUsers.Queries;

public class GetOrganizationUserRoleQueryTests : ApplicationIntegrationTestsBase
{
    private readonly ISender _sender;
    private readonly Faker _faker; 
    
    public GetOrganizationUserRoleQueryTests()
    {
        RegisterService(new ServiceForRegister(typeof(IApplicationDbAccessor), serviceProvider => new ApplicationDbAccessor(
            serviceProvider.GetRequiredService<ApplicationDbContext>()), ServiceLifetime.Scoped));
        RegisterService(new ServiceForRegister(typeof(IUnitOfWork), 
            serviceProvider => new UnitOfWork(serviceProvider.GetRequiredService<IApplicationDbAccessor>()), ServiceLifetime.Scoped));
        
        BuildServiceProvider();
        
        var dbContext = GetService<ApplicationDbContext>();
        dbContext.Database.Migrate();
        _sender = GetService<ISender>();
        _faker = new Faker();
    }
    
    [Theory]
    [InlineData(OrganizationUserRole.Admin)]
    [InlineData(OrganizationUserRole.Member)]
    public async Task Handle_UserIsNotInOrganization_ReturnsUserRole(OrganizationUserRole role)
    {
        // Arrange
        var dbContext = GetService<ApplicationDbContext>();
        var userId = Guid.NewGuid();
        var user = SetupUserInfo(userId);
        var organization = SetupOrganization();
        var organizationUser = SetupOrganizationUser(organization.Id, userId, role);
        
        await dbContext.Set<UserInfo>().AddAsync(user);
        await dbContext.Set<Organization>().AddAsync(organization);
        await dbContext.Set<OrganizationUser>().AddAsync(organizationUser);
        await dbContext.SaveChangesAsync();

        // Act
        var result = await _sender.Send(new GetOrganizationUserRoleQuery(organization.Id, userId));
        
        // Assert
        result.Should().Be(role);
    }
    
    [Fact]
    public async Task Handle_UserHasNoRoleInOrganization_OrganizationNotFoundException()
    {
        // Arrange
        var dbContext = GetService<ApplicationDbContext>();
        var userId = Guid.NewGuid();
        var user = SetupUserInfo(userId);
        var organization = SetupOrganization();
        
        await dbContext.Set<UserInfo>().AddAsync(user);
        await dbContext.Set<Organization>().AddAsync(organization);
        await dbContext.SaveChangesAsync();
        
        // Act & Assert;
        await Assert.ThrowsAsync<OrganizationUserNotFoundException>(() =>
            _sender.Send(new GetOrganizationUserRoleQuery(organization.Id, userId)));
    }
    
    private static UserInfo SetupUserInfo(Guid userId)
    {
        return new UserInfo
        {
            Id = userId
        };
    }

    private Organization SetupOrganization()
    {
        return new Organization
        {
            Id = Guid.NewGuid(),
            Name = _faker.Random.AlphaNumeric(8)
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