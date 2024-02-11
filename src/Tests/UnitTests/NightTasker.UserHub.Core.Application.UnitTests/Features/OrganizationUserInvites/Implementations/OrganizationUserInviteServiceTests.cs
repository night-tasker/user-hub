using FluentAssertions;
using NightTasker.UserHub.Core.Application.Exceptions.NotFound;
using NightTasker.UserHub.Core.Application.Features.Organizations.Services.Contracts;
using NightTasker.UserHub.Core.Application.Features.OrganizationUserInvites.Implementations;
using NightTasker.UserHub.Core.Application.Features.OrganizationUserInvites.Models;
using NightTasker.UserHub.Core.Domain.Repositories;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace NightTasker.UserHub.Core.Application.UnitTests.Features.OrganizationUserInvites.Implementations;

public class OrganizationUserInviteServiceTests
{
    private IUnitOfWork _unitOfWork = null!;
    private OrganizationUserInviteService _sut = null!;

    [SetUp]
    public void Setup()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _sut = new OrganizationUserInviteService(
            _unitOfWork,
            Substitute.For<IOrganizationService>());
    }

    [Test]
    public async Task AcceptInvite_InviteNotExist_OrganizationUserInviteForInvitedUserNotFoundException()
    {
        // Arrange
        var inviteDto = new AcceptOrganizationUserInviteDto(Guid.NewGuid(), Guid.NewGuid());
        _unitOfWork
            .OrganizationUserInviteRepository
            .TryGetIdForInvitedUser(inviteDto.InviteId, inviteDto.AcceptorUserId, CancellationToken.None)
            .ReturnsNull();
        var expectedException = new OrganizationUserInviteForInvitedUserNotFoundException(
            inviteDto.InviteId, inviteDto.AcceptorUserId);
        
        // Act && Assert
        var act = async () => await _sut.AcceptInvite(inviteDto, CancellationToken.None);
        var actualException = await act.Should().ThrowExactlyAsync<OrganizationUserInviteForInvitedUserNotFoundException>();
        actualException.WithMessage(expectedException.Message);
    }
    
    [Test]
    public async Task RevokeInvite_InviteNotExist_OrganizationUserInviteForInvitedUserNotFoundException()
    {
        // Arrange
        var inviteDto = new RevokeOrganizationUserInviteDto(Guid.NewGuid(), Guid.NewGuid());
        _unitOfWork
            .OrganizationUserInviteRepository
            .TryGetIdForInvitedUser(inviteDto.InviteId, inviteDto.RevokerUserId, CancellationToken.None)
            .ReturnsNull();
        var expectedException = new OrganizationUserInviteForInvitedUserNotFoundException(
            inviteDto.InviteId, inviteDto.RevokerUserId);
        
        // Act && Assert
        var act = async () => await _sut.RevokeInvite(inviteDto, CancellationToken.None);
        var actualException = await act.Should().ThrowExactlyAsync<OrganizationUserInviteForInvitedUserNotFoundException>();
        actualException.WithMessage(expectedException.Message);
    }
}