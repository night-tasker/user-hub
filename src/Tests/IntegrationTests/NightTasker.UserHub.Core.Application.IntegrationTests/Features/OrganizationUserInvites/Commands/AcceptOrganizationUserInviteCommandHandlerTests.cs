using Bogus;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NightTasker.Common.Core.Identity.Contracts;
using NightTasker.UserHub.Core.Application.Exceptions.NotFound;
using NightTasker.UserHub.Core.Application.Features.Organizations.Services.Contracts;
using NightTasker.UserHub.Core.Application.Features.Organizations.Services.Implementations;
using NightTasker.UserHub.Core.Application.Features.OrganizationUserInvites.Commands.AcceptInvite;
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

public class AcceptOrganizationUserInviteCommandHandlerTests : ApplicationIntegrationTestsBase
{
    private static readonly Guid UserId = Guid.NewGuid();
    private readonly Faker _faker;
    private readonly CancellationTokenSource _cancellationTokenSource;

    public AcceptOrganizationUserInviteCommandHandlerTests(TestNpgSql testNpgSql) : base(testNpgSql)
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
        RegisterService(new ServiceForRegister(typeof(AcceptOrganizationUserInviteCommandHandler)));
        
        BuildServiceProvider();
        
        PrepareDatabase();
        
        identityService.CurrentUserId.Returns(UserId);
        identityService.IsAuthenticated.Returns(true);
        
        _faker = new Faker();
        _cancellationTokenSource = new CancellationTokenSource();
    }
    
    [Fact]
    public async Task Handle_InviteNotExist_OrganizationUserInviteForInvitedUserNotFoundException()
    {
        // Arrange
        var command = new AcceptOrganizationUserInviteCommand(Guid.NewGuid(), UserId);
        var expectedException = new OrganizationUserInviteForInvitedUserNotFoundException(command.InviteId, command.AcceptorUserId);

        await using (var arrangeScope = CreateAsyncScope())
        {
            var dbContext = arrangeScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await dbContext.Set<Organization>()
                .AddAsync(Organization.CreateInstance(
                    Guid.NewGuid(), _faker.Company.CompanyName(), _faker.Company.CompanyName()),
                _cancellationTokenSource.Token);
            await dbContext.Set<User>().AddAsync(User.CreateInstance(UserId), _cancellationTokenSource.Token);
            await dbContext.SaveChangesAsync(_cancellationTokenSource.Token);
        }

        // Act & Assert
        await using (var actScope = CreateAsyncScope())
        {
            var sut = actScope.ServiceProvider.GetRequiredService<AcceptOrganizationUserInviteCommandHandler>();

            var act = async () => await sut.Handle(command, CancellationToken.None);
            var actualException = await act.Should().ThrowExactlyAsync<OrganizationUserInviteForInvitedUserNotFoundException>();
            actualException.WithMessage(expectedException.Message);
        }
    }
    
    [Fact]
    public async Task Handle_InviteExists_InviteIsAcceptedAndOrganizationUserIsCreated()
    {
        // Arrange
        var inviteId = Guid.NewGuid();
        var organizationId = Guid.NewGuid();
        var command = new AcceptOrganizationUserInviteCommand(inviteId, UserId);

        await using (var arrangeScope = CreateAsyncScope())
        {
            var dbContext = arrangeScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var organization = Organization.CreateInstance(
                organizationId, _faker.Company.CompanyName(), _faker.Company.CompanyName());
            await dbContext.Set<Organization>().AddAsync(organization, _cancellationTokenSource.Token);
            await dbContext.Set<User>().AddAsync(User.CreateInstance(UserId), _cancellationTokenSource.Token);
            var inviter = User.CreateInstance(Guid.NewGuid());
            await dbContext.Set<User>().AddAsync(inviter, _cancellationTokenSource.Token);
            var invite = OrganizationUserInvite.CreateInstance(
                command.InviteId, inviter.Id, UserId, organization.Id, _faker.Random.AlphaNumeric(32));
            await dbContext.Set<OrganizationUserInvite>().AddAsync(invite, _cancellationTokenSource.Token);
            await dbContext.SaveChangesAsync(_cancellationTokenSource.Token);
        }

        // Act
        await using (var actScope = CreateAsyncScope())
        {
            var sut = actScope.ServiceProvider.GetRequiredService<AcceptOrganizationUserInviteCommandHandler>();
            await sut.Handle(command, _cancellationTokenSource.Token);
        }
        
        // Assert
        await using (var assertScope = CreateAsyncScope())
        {
            var dbContext = assertScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var invite = await dbContext.Set<OrganizationUserInvite>().SingleOrDefaultAsync(
                x => x.Id == inviteId, _cancellationTokenSource.Token);
            invite.Should().NotBeNull();
            invite!.IsAccepted.Should().BeTrue();
            invite.IsRevoked.Should().BeNull();
            
            var organizationUsers = await dbContext.Set<OrganizationUser>().ToListAsync(_cancellationTokenSource.Token);
            organizationUsers.Should().HaveCount(1);
            var organizationUser = organizationUsers.First();
            organizationUser.OrganizationId.Should().Be(organizationId);
            organizationUser.UserId.Should().Be(UserId);
            organizationUser.Role.Should().Be(OrganizationUserRole.Member);
        }
    }
}