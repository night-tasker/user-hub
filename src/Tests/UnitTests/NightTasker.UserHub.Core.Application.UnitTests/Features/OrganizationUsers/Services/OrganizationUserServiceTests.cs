﻿using NightTasker.UserHub.Core.Application.Exceptions.NotFound;
using NightTasker.UserHub.Core.Application.Features.OrganizationUsers.Models;
using NightTasker.UserHub.Core.Application.Features.OrganizationUsers.Services.Implementations;
using NightTasker.UserHub.Core.Domain.Enums;
using NightTasker.UserHub.Core.Domain.Repositories;
using NSubstitute;

namespace NightTasker.UserHub.Core.Application.UnitTests.Features.OrganizationUsers.Services;

public class OrganizationUserServiceTests
{
    private IUnitOfWork _unitOfWork = null!;
    private OrganizationUserService _sut = null!;

    [SetUp]
    public void Setup()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _sut = new OrganizationUserService(_unitOfWork);
    }

    [Test]
    public void CreateOrganizationUserWithOutSaving_UserNotExist_UserNotFoundException()
    {
        // Arrange
        var dto = new CreateOrganizationUserDto(Guid.NewGuid(), Guid.NewGuid(), OrganizationUserRole.Admin);

        _unitOfWork.OrganizationRepository
            .CheckExistsByIdForUser(dto.UserId, dto.OrganizationId, CancellationToken.None)
            .Returns(true);
        
        _unitOfWork.UserRepository
            .CheckExistsById(dto.UserId, CancellationToken.None)
            .Returns(false);
        
        // Act && Assert
        Assert.ThrowsAsync<UserNotFoundException>(() => _sut.CreateOrganizationUser(dto, CancellationToken.None));
    }
    
    [Test]
    public void CreateOrganizationUserWithOutSaving_OrganizationNotExist_OrganizationNotFoundException()
    {
        // Arrange
        var dto = new CreateOrganizationUserDto(Guid.NewGuid(), Guid.NewGuid(), OrganizationUserRole.Admin);

        _unitOfWork.OrganizationRepository
            .CheckExistsByIdForUser(dto.UserId, dto.OrganizationId,  CancellationToken.None)
            .Returns(false);
        
        _unitOfWork.UserRepository
            .CheckExistsById(dto.UserId, CancellationToken.None)
            .Returns(true);
        
        // Act && Assert
        Assert.ThrowsAsync<OrganizationNotFoundException>(() => _sut.CreateOrganizationUser(dto, CancellationToken.None));
    }
}