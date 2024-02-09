using Bogus;
using FluentAssertions;
using NightTasker.UserHub.Core.Application.Exceptions.NotFound;
using NightTasker.UserHub.Core.Application.Exceptions.Unauthorized;
using NightTasker.UserHub.Core.Application.Features.Organizations.Models;
using NightTasker.UserHub.Core.Application.Features.Organizations.Services.Implementations;
using NightTasker.UserHub.Core.Domain.Entities;
using NightTasker.UserHub.Core.Domain.Enums;
using NightTasker.UserHub.Core.Domain.Repositories;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace NightTasker.UserHub.Core.Application.UnitTests.Features.Organizations.Services;

public class OrganizationServiceTests
{
    private IUnitOfWork _unitOfWork = null!;
    private OrganizationService _sut = null!;
    private Faker _faker = null!;
    private CancellationTokenSource _cancellationTokenSource = null!;

    [SetUp]
    public void Setup()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _sut = new OrganizationService(_unitOfWork);
        _faker = new Faker();
        _cancellationTokenSource = new CancellationTokenSource();
    }

    [Test]
    public async Task UpdateOrganization_OrganizationDoesNotExist_ThrowsOrganizationUserNotFoundException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var organizationId = Guid.NewGuid();
        _unitOfWork.OrganizationRepository
            .TryGetOrganizationForUser(userId, organizationId, true, _cancellationTokenSource.Token)
            .ReturnsNull();
        
        var updateOrganizationDto = new UpdateOrganizationDto(
            _faker.Random.AlphaNumeric(8), _faker.Random.AlphaNumeric(24));
        
        // Act
        var func = async () => await _sut.UpdateOrganizationAsUser(userId, organizationId, updateOrganizationDto, _cancellationTokenSource.Token);
        
        // Assert
        await func.Should().ThrowAsync<OrganizationUserNotFoundException>();
    }
    
    [Test]
    public async Task UpdateOrganization_UserIsNotInOrganization_ThrowsOrganizationUserNotFoundException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var organizationId = Guid.NewGuid();
        
        _unitOfWork.OrganizationRepository
            .TryGetOrganizationForUser(userId, organizationId, true, _cancellationTokenSource.Token)
            .Returns(Organization.CreateInstance(organizationId, null, null ));
        
        _unitOfWork.OrganizationUserRepository
            .TryGetUserOrganizationRole(organizationId, userId, _cancellationTokenSource.Token)
            .ReturnsNull();
        
        var updateOrganizationDto = new UpdateOrganizationDto(
            _faker.Random.AlphaNumeric(8), _faker.Random.AlphaNumeric(24));
        
        // Act
        var func = async () => await _sut.UpdateOrganizationAsUser(userId, organizationId, updateOrganizationDto, _cancellationTokenSource.Token);
        
        // Assert
        await func.Should().ThrowAsync<OrganizationUserNotFoundException>();
    }
    
    [Test]
    public async Task UpdateOrganization_UserIsNotAdminInOrganization_ThrowsUserCanNotUpdateOrganizationUnauthorizedException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var organizationId = Guid.NewGuid();
        
        _unitOfWork.OrganizationRepository
            .TryGetOrganizationForUser(userId, organizationId, true, _cancellationTokenSource.Token)
            .Returns(Organization.CreateInstance(organizationId, null, null));
        
        _unitOfWork.OrganizationUserRepository
            .TryGetUserOrganizationRole(organizationId, userId, _cancellationTokenSource.Token)
            .Returns(OrganizationUserRole.Member);
        
        var updateOrganizationDto = new UpdateOrganizationDto(
            _faker.Random.AlphaNumeric(8), _faker.Random.AlphaNumeric(24));
        
        // Act
        var func = async () => await _sut.UpdateOrganizationAsUser(userId, organizationId, updateOrganizationDto, _cancellationTokenSource.Token);
        
        // Assert
        await func.Should().ThrowAsync<UserCanNotUpdateOrganizationUnauthorizedException>();
    }
    
    [Test]
    public async Task RemoveOrganization_OrganizationDoesNotExist_ThrowsOrganizationUserNotFoundException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var organizationId = Guid.NewGuid();
        _unitOfWork.OrganizationRepository
            .TryGetOrganizationForUser(userId, organizationId, true, _cancellationTokenSource.Token)
            .ReturnsNull();

        // Act
        var func = async () => await _sut.RemoveOrganizationAsUser(userId, organizationId, _cancellationTokenSource.Token);
        
        // Assert
        await func.Should().ThrowAsync<OrganizationUserNotFoundException>();
    }
    
    [Test]
    public async Task RemoveOrganization_UserIsNotInOrganization_ThrowsOrganizationUserNotFoundException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var organizationId = Guid.NewGuid();
            
        _unitOfWork.OrganizationRepository
            .TryGetOrganizationForUser(userId, organizationId, true, _cancellationTokenSource.Token)
            .Returns(Organization.CreateInstance(organizationId, null, null));
        
        _unitOfWork.OrganizationUserRepository
            .TryGetUserOrganizationRole(organizationId, userId, _cancellationTokenSource.Token)
            .ReturnsNull();

        // Act
        var func = async () => await _sut.RemoveOrganizationAsUser(userId, organizationId, _cancellationTokenSource.Token);
        
        // Assert
        await func.Should().ThrowAsync<OrganizationUserNotFoundException>();
    }
    
    [Test]
    public async Task RemoveOrganization_UserIsNotAdminInOrganization_ThrowsUserCanNotDeleteOrganizationUnauthorizedException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var organizationId = Guid.NewGuid();
        
        _unitOfWork.OrganizationRepository
            .TryGetOrganizationForUser(userId, organizationId, false, _cancellationTokenSource.Token)
            .Returns(Organization.CreateInstance(organizationId, null, null));
        
        _unitOfWork.OrganizationUserRepository
            .TryGetUserOrganizationRole(organizationId, userId, _cancellationTokenSource.Token)
            .Returns(OrganizationUserRole.Member);

        // Act
        var func = async () => await _sut.RemoveOrganizationAsUser(userId, organizationId, _cancellationTokenSource.Token);
        
        // Assert
        await func.Should().ThrowAsync<UserCanNotDeleteOrganizationUnauthorizedException>();
    }
}