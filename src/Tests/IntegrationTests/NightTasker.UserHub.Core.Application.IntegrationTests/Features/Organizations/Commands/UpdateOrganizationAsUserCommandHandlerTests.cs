using Bogus;
using FluentAssertions;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NightTasker.Common.Core.Identity.Contracts;
using NightTasker.UserHub.Core.Application.ApplicationContracts.Repository;
using NightTasker.UserHub.Core.Application.Features.Organizations.Commands.UpdateOrganizationAsUser;
using NightTasker.UserHub.Core.Application.Features.Organizations.Models;
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
                serviceProvider.GetRequiredService<IUnitOfWork>(), 
                serviceProvider.GetRequiredService<IMapper>()), ServiceLifetime.Scoped));
        
        RegisterService(new ServiceForRegister(typeof(IOrganizationUserService), 
            serviceProvider => new OrganizationUserService(
                serviceProvider.GetRequiredService<IUnitOfWork>(), 
                serviceProvider.GetRequiredService<IMapper>()), ServiceLifetime.Scoped));
        
        RegisterService(new ServiceForRegister(typeof(UpdateOrganizationAsUserCommandHandler)));
        
        BuildServiceProvider();
        
        identityService.CurrentUserId.Returns(UserId);
        identityService.IsAuthenticated.Returns(true);

        var dbContext = GetService<ApplicationDbContext>();
        dbContext.Database.Migrate();
        _faker = new Faker();
        _cancellationTokenSource = new CancellationTokenSource();
    }
    
    [Fact]
    public async Task UpdateOrganization_OrganizationExists_OrganizationUpdated()
    {
        // Arrange
        var dbContext = GetService<ApplicationDbContext>();
        var organization = SetupRandomOrganization();
        await dbContext.Set<Organization>().AddAsync(organization, _cancellationTokenSource.Token);
        await dbContext.Set<UserInfo>().AddAsync(SetupUserInfo(UserId), _cancellationTokenSource.Token);
        var organizationUser = SetupOrganizationUser(organization.Id, UserId, OrganizationUserRole.Admin);
        await dbContext.Set<OrganizationUser>().AddAsync(organizationUser, _cancellationTokenSource.Token);
        await dbContext.SaveChangesAsync(_cancellationTokenSource.Token);
        
        var updateOrganizationDto = new UpdateOrganizationDto(
            _faker.Random.AlphaNumeric(8), _faker.Random.AlphaNumeric(32));
        var updateOrganizationAsUserCommand = new UpdateOrganizationAsUserCommand(
            UserId,organization.Id, updateOrganizationDto);
        var sut = GetService<UpdateOrganizationAsUserCommandHandler>();
        
        // Act
        await sut.Handle(updateOrganizationAsUserCommand, _cancellationTokenSource.Token);
        
        // Assert
        var updatedOrganization = await dbContext.Set<Organization>().SingleOrDefaultAsync(_cancellationTokenSource.Token);
        
        updatedOrganization.Should().NotBeNull();
        updatedOrganization!.Id.Should().Be(organization.Id);
        updatedOrganization.Name.Should().Be(updateOrganizationDto.Name);
        updatedOrganization.Description.Should().Be(updateOrganizationDto.Description);
    }

    private static UserInfo SetupUserInfo(Guid userId)
    {
        return new UserInfo
        {
            Id = userId
        };
    }

    private Organization SetupRandomOrganization()
    {
        return new Organization
        {
            Id = Guid.NewGuid(),
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