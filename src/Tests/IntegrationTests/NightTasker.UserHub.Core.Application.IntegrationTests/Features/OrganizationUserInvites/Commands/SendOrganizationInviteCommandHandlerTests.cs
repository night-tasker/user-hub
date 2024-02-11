using Bogus;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NightTasker.Common.Core.Identity.Contracts;
using NightTasker.UserHub.Core.Application.Exceptions.NotFound;
using NightTasker.UserHub.Core.Application.Exceptions.Unauthorized;
using NightTasker.UserHub.Core.Application.Features.Organizations.Services.Contracts;
using NightTasker.UserHub.Core.Application.Features.Organizations.Services.Implementations;
using NightTasker.UserHub.Core.Application.Features.OrganizationUserInvites.Commands.SendInvite;
using NightTasker.UserHub.Core.Application.Features.OrganizationUserInvites.Contracts;
using NightTasker.UserHub.Core.Application.Features.OrganizationUserInvites.Implementations;
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

namespace NightTasker.UserHub.Core.Application.IntegrationTests.Features.OrganizationUserInvites.Commands;

public sealed class SendOrganizationInviteCommandHandlerTests : ApplicationIntegrationTestsBase
{
    private static readonly Guid UserId = Guid.NewGuid();
    private readonly Faker _faker;
    private readonly CancellationTokenSource _cancellationTokenSource;

    public SendOrganizationInviteCommandHandlerTests(TestNpgSql testNpgSql) : base(testNpgSql)
    {
        var identityService = Substitute.For<IIdentityService>();
        RegisterService(new ServiceForRegister(typeof(IApplicationDataAccessor), serviceProvider => new ApplicationDataAccessor(
            serviceProvider.GetRequiredService<ApplicationDbContext>()), ServiceLifetime.Scoped));
        RegisterService(new ServiceForRegister(typeof(IUnitOfWork), 
            serviceProvider => new UnitOfWork(serviceProvider.GetRequiredService<IApplicationDataAccessor>()), ServiceLifetime.Scoped));
        RegisterService(new ServiceForRegister(typeof(IOrganizationService), 
            serviceProvider => new OrganizationService(
                serviceProvider.GetRequiredService<IUnitOfWork>()), ServiceLifetime.Scoped));
        
        RegisterService(new ServiceForRegister(typeof(IOrganizationUserInviteService), 
            serviceProvider => new OrganizationUserInviteService(
                serviceProvider.GetRequiredService<IUnitOfWork>(), 
                serviceProvider.GetRequiredService<IOrganizationService>()), ServiceLifetime.Scoped));
        RegisterService(new ServiceForRegister(typeof(SendOrganizationUserInviteCommandHandler)));
        
        BuildServiceProvider();
        
        identityService.CurrentUserId.Returns(UserId);
        identityService.IsAuthenticated.Returns(true);

        PrepareDatabase();
        
        _faker = new Faker();
        _cancellationTokenSource = new CancellationTokenSource();
    }
    
    [Fact]
    public async Task Handle_InviterIsAdminInOrganization_OrganizationHasInvite()
    {
        // Arrange
        var invitedUserId = Guid.NewGuid();
        var organizationId = Guid.NewGuid();
        var inviteMessage = _faker.Random.AlphaNumeric(32);
        await using (var arrangeScope = CreateAsyncScope())
        {
            var dbContext = arrangeScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Set<User>().Add(SetupUser(UserId));
            dbContext.Set<User>().Add(SetupUser(invitedUserId));
            var organization = SetupRandomOrganization(organizationId);
            dbContext.Set<Organization>().Add(organization);
            dbContext.Set<OrganizationUser>()
                .Add(SetupOrganizationUser(organization.Id, UserId, OrganizationUserRole.Admin));
            await dbContext.SaveChangesAsync();
        }
        
        // Act
        await using (var actScope = CreateAsyncScope())
        {
            var sut = actScope.ServiceProvider.GetRequiredService<SendOrganizationUserInviteCommandHandler>();
            var sendOrganizationUserInviteCommand = new SendOrganizationUserInviteCommand(
                UserId, invitedUserId, organizationId, inviteMessage);
            await sut.Handle(sendOrganizationUserInviteCommand, _cancellationTokenSource.Token);
        }
        
        // Assert
        await using (var assertScope = CreateAsyncScope())
        {
            var dbContext = assertScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var organization = await dbContext
                .Set<Organization>()
                .Include(o => o.OrganizationUserInvites)
                .SingleAsync(_cancellationTokenSource.Token);

            organization.OrganizationUserInvites.Count.Should().Be(1);
            var invite = organization.OrganizationUserInvites.SingleOrDefault();
            invite.Should().NotBeNull();
            invite!.Message.Should().Be(inviteMessage);
            invite.InviterUserId.Should().Be(UserId);
            invite.InvitedUserId.Should().Be(invitedUserId);
        }
    }

    [Fact]
    public async Task Handle_InviterIsNotInOrganization_ThrowsOrganizationNotFoundException()
    {
        // Arrange
        var invitedUserId = Guid.NewGuid();
        var organizationId = Guid.NewGuid();
        var inviteMessage = _faker.Random.AlphaNumeric(32);
        var expectedException = new OrganizationUserNotFoundException(organizationId, UserId);
        await using (var arrangeScope = CreateAsyncScope())
        {
            var dbContext = arrangeScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Set<User>().Add(SetupUser(UserId));
            dbContext.Set<User>().Add(SetupUser(invitedUserId));
            var organization = SetupRandomOrganization(organizationId);
            dbContext.Set<Organization>().Add(organization);
            await dbContext.SaveChangesAsync();
        }
        
        // Act & Assert
        await using (var actScope = CreateAsyncScope())
        {
            var sut = actScope.ServiceProvider.GetRequiredService<SendOrganizationUserInviteCommandHandler>();
            var sendOrganizationUserInviteCommand = new SendOrganizationUserInviteCommand(
                UserId, invitedUserId, organizationId, inviteMessage);
            var act = async () => await sut.Handle(sendOrganizationUserInviteCommand, _cancellationTokenSource.Token);
            var actualException = await act.Should().ThrowAsync<OrganizationUserNotFoundException>();
            actualException.WithMessage(expectedException.Message);
        }
    }
    
    [Fact]
    public async Task Handle_InviterIsMemberInOrganization_ThrowsUserCanNotUpdateOrganizationUnauthorizedException()
    {
        // Arrange
        var invitedUserId = Guid.NewGuid();
        var organizationId = Guid.NewGuid();
        var inviteMessage = _faker.Random.AlphaNumeric(32);
        var expectedException = new UserCanNotUpdateOrganizationUnauthorizedException(organizationId, UserId);
        await using (var arrangeScope = CreateAsyncScope())
        {
            var dbContext = arrangeScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Set<User>().Add(SetupUser(UserId));
            dbContext.Set<User>().Add(SetupUser(invitedUserId));
            var organization = SetupRandomOrganization(organizationId);
            dbContext.Set<Organization>().Add(organization);
            dbContext.Set<OrganizationUser>().Add(
                SetupOrganizationUser(organizationId, UserId, OrganizationUserRole.Member));
            await dbContext.SaveChangesAsync();
        }
        
        // Act & Assert
        await using (var actScope = CreateAsyncScope())
        {
            var sut = actScope.ServiceProvider.GetRequiredService<SendOrganizationUserInviteCommandHandler>();
            var sendOrganizationUserInviteCommand = new SendOrganizationUserInviteCommand(
                UserId, invitedUserId, organizationId, inviteMessage);
            var act = async () => await sut.Handle(sendOrganizationUserInviteCommand, _cancellationTokenSource.Token);
            var actualException = await act.Should().ThrowAsync<UserCanNotUpdateOrganizationUnauthorizedException>();
            actualException.WithMessage(expectedException.Message);
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