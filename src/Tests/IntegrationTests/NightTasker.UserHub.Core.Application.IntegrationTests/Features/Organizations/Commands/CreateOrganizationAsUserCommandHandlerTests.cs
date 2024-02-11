using Bogus;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NightTasker.Common.Core.Identity.Contracts;
using NightTasker.UserHub.Core.Application.Features.Organizations.Commands.CreateOrganizationAsUser;
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

public class CreateOrganizationAsUserCommandHandlerTests : ApplicationIntegrationTestsBase
{
    private static readonly Guid UserId = Guid.NewGuid();
    private readonly Faker _faker;
    private readonly CancellationTokenSource _cancellationTokenSource;

    public CreateOrganizationAsUserCommandHandlerTests(TestNpgSql testNpgSql) : base(testNpgSql)
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
        RegisterService(new ServiceForRegister(typeof(CreateOrganizationAsUserCommandHandler)));
        
        BuildServiceProvider();
        
        identityService.CurrentUserId.Returns(UserId);
        identityService.IsAuthenticated.Returns(true);
        PrepareDatabase();
        
        _faker = new Faker();
        _cancellationTokenSource = new CancellationTokenSource();
    }
                                                
    [Fact]
    public async Task Handle_CreateOrganization_OrganizationHasAdminWithCurrentUserId()
    {
        // Arrange
        var dbContext = GetService<ApplicationDbContext>();
        dbContext.Set<User>().Add(SetupUser(UserId));
        await dbContext.SaveChangesAsync();

        var createOrganizationAsUserCommand = new CreateOrganizationAsUserCommand(
                _faker.Random.AlphaNumeric(8), 
                _faker.Random.AlphaNumeric(32), 
                UserId);
        var sut = GetService<CreateOrganizationAsUserCommandHandler>();
        
        // Act
        var result = await sut.Handle(createOrganizationAsUserCommand, _cancellationTokenSource.Token);
        
        // Assert
        var organizations = await dbContext.Set<Organization>().ToListAsync();
        
        organizations.Should().HaveCount(1);
        organizations[0].Should().NotBeNull();
        organizations[0].Id.Should().Be(result);
        organizations[0].Name.Should().Be(createOrganizationAsUserCommand.Name);
        organizations[0].Description.Should().Be(createOrganizationAsUserCommand.Description);
        
        var organizationUsers = await dbContext.Set<OrganizationUser>().ToListAsync();
        
        organizationUsers.Should().HaveCount(1);
        organizationUsers[0].UserId.Should().Be(UserId);
        organizationUsers[0].OrganizationId.Should().Be(result);
        organizationUsers[0].Role.Should().Be(OrganizationUserRole.Admin);
    }

    private static User SetupUser(Guid userId)
    {
        return User.CreateInstance(userId);
    }
}