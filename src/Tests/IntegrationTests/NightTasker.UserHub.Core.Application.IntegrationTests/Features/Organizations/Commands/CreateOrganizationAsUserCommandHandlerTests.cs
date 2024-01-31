using Bogus;
using FluentAssertions;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NightTasker.Common.Core.Identity.Contracts;
using NightTasker.UserHub.Core.Application.ApplicationContracts.Repository;
using NightTasker.UserHub.Core.Application.Features.Organizations.Commands.CreateOrganization;
using NightTasker.UserHub.Core.Application.Features.Organizations.Services.Contracts;
using NightTasker.UserHub.Core.Application.Features.Organizations.Services.Implementations;
using NightTasker.UserHub.Core.Application.Features.OrganizationUsers.Services.Contracts;
using NightTasker.UserHub.Core.Application.Features.OrganizationUsers.Services.Implementations;
using NightTasker.UserHub.Core.Domain.Entities;
using NightTasker.UserHub.Core.Domain.Enums;
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
    private readonly ISender _sender;
    private static readonly Guid UserId = Guid.NewGuid();
    private readonly Faker _faker; 
    
    public CreateOrganizationAsUserCommandHandlerTests()
    {
        var identityService = Substitute.For<IIdentityService>();
        RegisterService(new ServiceForRegister(typeof(IApplicationDbAccessor), serviceProvider => new ApplicationDbAccessor(
            serviceProvider.GetRequiredService<ApplicationDbContext>()), ServiceLifetime.Scoped));
        RegisterService(new ServiceForRegister(typeof(IUnitOfWork), 
            serviceProvider => new UnitOfWork(serviceProvider.GetRequiredService<IApplicationDbAccessor>()), ServiceLifetime.Scoped));
        RegisterService(new ServiceForRegister(typeof(IOrganizationService), 
            serviceProvider => new OrganizationService(
                serviceProvider.GetRequiredService<IUnitOfWork>(), 
                serviceProvider.GetRequiredService<IMapper>()), ServiceLifetime.Scoped));
        
        RegisterService(new ServiceForRegister(typeof(IOrganizationUserService), 
            serviceProvider => new OrganizationUserService(
                serviceProvider.GetRequiredService<IUnitOfWork>(), 
                serviceProvider.GetRequiredService<IMapper>()), ServiceLifetime.Scoped));
        
        BuildServiceProvider();
        
        identityService.CurrentUserId.Returns(UserId);
        identityService.IsAuthenticated.Returns(true);

        var dbContext = GetService<ApplicationDbContext>();
        dbContext.Database.Migrate();
        _sender = GetService<ISender>();
        _faker = new Faker();
    }

    [Fact]
    public async Task Handle_CreateOrganization_OrganizationHasAdminWithCurrentUserId()
    {
        // Arrange
        var dbContext = GetService<ApplicationDbContext>();
        dbContext.Set<UserInfo>().Add(SetupUserInfo(UserId));
        await dbContext.SaveChangesAsync();

        var createOrganizationAsUserCommand = new CreateOrganizationAsUserCommand(
                _faker.Random.AlphaNumeric(8), 
                _faker.Random.AlphaNumeric(32), 
                UserId);
        
        // Act
        var result = await _sender.Send(createOrganizationAsUserCommand);
        
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

    private static UserInfo SetupUserInfo(Guid userId)
    {
        return new UserInfo
        {
            Id = userId
        };
    }
}