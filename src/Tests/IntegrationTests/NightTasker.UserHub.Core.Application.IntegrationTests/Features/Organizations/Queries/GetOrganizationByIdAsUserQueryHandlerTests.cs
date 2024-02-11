using Bogus;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NightTasker.Common.Core.Identity.Contracts;
using NightTasker.UserHub.Core.Application.Exceptions.NotFound;
using NightTasker.UserHub.Core.Application.Features.Organizations.Queries.GetOrganizationById;
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

public class GetOrganizationByIdAsUserQueryHandlerTests : ApplicationIntegrationTestsBase
{
    private static readonly Guid UserId = Guid.NewGuid();
    private readonly Faker _faker;
    private readonly CancellationTokenSource _cancellationTokenSource;

    public GetOrganizationByIdAsUserQueryHandlerTests(TestNpgSql testNpgSql) : base(testNpgSql)
    {
        var identityService = Substitute.For<IIdentityService>();
        RegisterService(new ServiceForRegister(typeof(IApplicationDataAccessor), serviceProvider => new ApplicationDataAccessor(
            serviceProvider.GetRequiredService<ApplicationDbContext>()), ServiceLifetime.Scoped));
        RegisterService(new ServiceForRegister(typeof(IUnitOfWork), 
            serviceProvider => new UnitOfWork(serviceProvider.GetRequiredService<IApplicationDataAccessor>()), ServiceLifetime.Scoped));
        RegisterService(new ServiceForRegister(typeof(GetOrganizationByIdAsUserQueryHandler)));
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
    public async Task Handle_UserWithRoleInOrganization_ReturnsRightUsersCount(OrganizationUserRole currentUserRoleInOrganization)
    {
        // Arrange
        var dbContext = GetService<ApplicationDbContext>();
        var users = new List<User>
        {
            SetupUser(UserId), SetupUser(Guid.NewGuid()), SetupUser(Guid.NewGuid()), SetupUser(Guid.NewGuid())
        };
        
        dbContext.Set<User>().AddRange(users);
        
        var organization = SetupOrganization();
        dbContext.Set<Organization>().Add(organization);
        
        var organizationUsers = new List<OrganizationUser>
        {
            SetupOrganizationUser(organization.Id, users[0].Id, currentUserRoleInOrganization),
            SetupOrganizationUser(organization.Id, users[1].Id, OrganizationUserRole.Member),
            SetupOrganizationUser(organization.Id, users[2].Id, OrganizationUserRole.Admin),
            SetupOrganizationUser(organization.Id, users[3].Id, OrganizationUserRole.Member)
        };
        dbContext.Set<OrganizationUser>().AddRange(organizationUsers);
        await dbContext.SaveChangesAsync();
        
        var query = new GetOrganizationByIdAsUserQuery(organization.Id, UserId);
        var sut = GetService<GetOrganizationByIdAsUserQueryHandler>();        
        
        // Act
        var result = await sut.Handle(query, _cancellationTokenSource.Token);

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

        var query = new GetOrganizationByIdAsUserQuery(notExistingOrganizationId, UserId);
        var sut = GetService<GetOrganizationByIdAsUserQueryHandler>();    
        
        // Act
        Func<Task> act = async () => await sut.Handle(query, _cancellationTokenSource.Token);
        
        // Assert
        await act.Should().ThrowAsync<OrganizationUserNotFoundException>();
    }
    
    [Fact]
    public async Task Handle_OrganizationExistsButUserIsNotOrganizationUser_OrganizationNotFoundException()
    {
        // Arrange
        var dbContext = GetService<ApplicationDbContext>();
        dbContext.Set<User>().Add(SetupUser(UserId));
        var organization = SetupOrganization();
        dbContext.Set<Organization>().Add(organization);
        await dbContext.SaveChangesAsync();

        var query = new GetOrganizationByIdAsUserQuery(organization.Id, UserId);
        var sut = GetService<GetOrganizationByIdAsUserQueryHandler>();
        
        // Act
        Func<Task> act = async () => await sut.Handle(query, _cancellationTokenSource.Token);
        
        // Assert
        await act.Should().ThrowAsync<OrganizationUserNotFoundException>();
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