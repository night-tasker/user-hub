using Bogus;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NightTasker.UserHub.Core.Application.Exceptions.NotFound;
using NightTasker.UserHub.Core.Application.Features.OrganizationUsers.Queries.GetOrganizationUserRole;
using NightTasker.UserHub.Core.Domain.Entities;
using NightTasker.UserHub.Core.Domain.Enums;
using NightTasker.UserHub.Core.Domain.Repositories;
using NightTasker.UserHub.Infrastructure.Persistence;
using NightTasker.UserHub.Infrastructure.Persistence.Contracts;
using NightTasker.UserHub.Infrastructure.Persistence.Repository.Common;
using NightTasker.UserHub.IntegrationTests.Framework;
using NightTasker.UserHub.Presentation.WebApi.Implementations;
using Xunit;

namespace NightTasker.UserHub.Core.Application.IntegrationTests.Features.OrganizationUsers.Queries;

public class GetOrganizationUserRoleQueryTests : ApplicationIntegrationTestsBase
{
    private readonly Faker _faker;
    private readonly CancellationTokenSource _cancellationTokenSource;

    public GetOrganizationUserRoleQueryTests(TestNpgSql testNpgSql) : base(testNpgSql)
    {
        RegisterService(new ServiceForRegister(typeof(IApplicationDataAccessor), serviceProvider => new ApplicationDataAccessor(
            serviceProvider.GetRequiredService<ApplicationDbContext>()), ServiceLifetime.Scoped));
        RegisterService(new ServiceForRegister(typeof(IUnitOfWork), 
            serviceProvider => new UnitOfWork(serviceProvider.GetRequiredService<IApplicationDataAccessor>()), ServiceLifetime.Scoped));
        RegisterService(new ServiceForRegister(typeof(GetOrganizationUserRoleQueryHandler)));
        
        BuildServiceProvider();
        
        PrepareDatabase();
        
        _faker = new Faker();
        _cancellationTokenSource = new CancellationTokenSource();
    }
    
    [Theory]
    [InlineData(OrganizationUserRole.Admin)]
    [InlineData(OrganizationUserRole.Member)]
    public async Task Handle_UserIsNotInOrganization_ReturnsUserRole(OrganizationUserRole role)
    {
        // Arrange
        var dbContext = GetService<ApplicationDbContext>();
        var userId = Guid.NewGuid();
        var user = SetupUser(userId);
        var organization = SetupOrganization();
        var organizationUser = SetupOrganizationUser(organization.Id, userId, role);
        
        await dbContext.Set<User>().AddAsync(user);
        await dbContext.Set<Organization>().AddAsync(organization);
        await dbContext.Set<OrganizationUser>().AddAsync(organizationUser);
        await dbContext.SaveChangesAsync();
        var query = new GetOrganizationUserRoleQuery(organization.Id, userId);
        var sut = GetService<GetOrganizationUserRoleQueryHandler>();

        // Act
        var result = await sut.Handle(query, _cancellationTokenSource.Token);
        
        // Assert
        result.Should().Be(role);
    }
    
    [Fact]
    public async Task Handle_UserHasNoRoleInOrganization_OrganizationNotFoundException()
    {
        // Arrange
        var dbContext = GetService<ApplicationDbContext>();
        var userId = Guid.NewGuid();
        var user = SetupUser(userId);
        var organization = SetupOrganization();
        
        await dbContext.Set<User>().AddAsync(user);
        await dbContext.Set<Organization>().AddAsync(organization);
        await dbContext.SaveChangesAsync();
        
        var sut = GetService<GetOrganizationUserRoleQueryHandler>();
        var query = new GetOrganizationUserRoleQuery(organization.Id, userId);
        
        // Act
        var func = async () => await sut.Handle(query, _cancellationTokenSource.Token);
        
        // Assert
        await func.Should().ThrowAsync<OrganizationUserNotFoundException>();
    }
    
    private static User SetupUser(Guid userId)
    {
        return User.CreateInstance(userId);
    }

    private Organization SetupOrganization()
    {
        return Organization.CreateInstance(_faker.Random.AlphaNumeric(8), null);
    }

    private static OrganizationUser SetupOrganizationUser(Guid organizationId, Guid userId, OrganizationUserRole role)
    {
        return OrganizationUser.CreateInstance(organizationId, userId, role);
    }
}