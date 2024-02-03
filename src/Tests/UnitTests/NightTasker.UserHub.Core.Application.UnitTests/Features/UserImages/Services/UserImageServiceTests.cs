using Bogus;
using FluentAssertions;
using NightTasker.UserHub.Core.Application.ApplicationContracts.Services;
using NightTasker.UserHub.Core.Application.Exceptions.NotFound;
using NightTasker.UserHub.Core.Application.Features.UserImages.Models;
using NightTasker.UserHub.Core.Application.Features.UserImages.Services.Implementations;
using NightTasker.UserHub.Core.Domain.Repositories;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace NightTasker.UserHub.Core.Application.UnitTests.Features.UserImages.Services;

public class UserImageServiceTests
{
    private IUnitOfWork _unitOfWork = null!;
    private UserImageService _sut = null!;
    private Faker _faker = null!;

    [SetUp]
    public void Setup()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _sut = new UserImageService(_unitOfWork, Substitute.For<IStorageFileService>());
        _faker = new Faker();
    }

    [Test]
    public void CreateUserImage_UserNotExist_ThrowsUserNotFoundException()
    {
        // Arrange
        var createUserImageDto = new CreateUserImageDto(
            Guid.NewGuid(), _faker.Random.AlphaNumeric(8), "png", "image/png", 100);
       
        _unitOfWork.UserRepository
            .TryGetById(createUserImageDto.UserId, false, Arg.Any<CancellationToken>())
            .ReturnsNull();

        // Act & Assert
        Assert.ThrowsAsync<UserNotFoundException>(
            () => _sut.CreateUserImage(createUserImageDto, CancellationToken.None));
    }

    [Test]
    public void DownloadActiveUserImageByUserId_ActiveImageNotExist_ThrowsActiveUserImageForUserIdNotFoundException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _unitOfWork.UserImageRepository
            .TryGetActiveImageByUserId(userId, false, Arg.Any<CancellationToken>())
            .ReturnsNull();
        
        // Act & Assert
        Assert.ThrowsAsync<ActiveUserImageForUserIdNotFoundException>(
            () => _sut.DownloadActiveUserImageByUserId(userId, CancellationToken.None));
    }
    
    [Test]
    public void GetUserActiveImageUrlByUserId_ActiveImageNotExist_ThrowsActiveUserImageForUserIdNotFoundException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _unitOfWork.UserImageRepository
            .TryGetActiveImageByUserId(userId, false, Arg.Any<CancellationToken>())
            .ReturnsNull();
        
        // Act & Assert
        Assert.ThrowsAsync<ActiveUserImageForUserIdNotFoundException>(
            () => _sut.DownloadActiveUserImageByUserId(userId, CancellationToken.None));
    }

    [Test]
    public async Task GetUserImagesByUserId_UserImagesNotExist_EmptyCollection()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _unitOfWork.UserImageRepository
            .GetImageIdsWithActiveByUserId(userId, Arg.Any<CancellationToken>())
            .Returns(new Dictionary<Guid, bool>());
        
        // Act
        var result = await _sut.GetUserImagesByUserId(userId, CancellationToken.None);
        
        // Assert
        result.Should().BeEmpty();
    }

    [Test]
    public void RemoveUserImageById_UserImageNotExist_ThrowsUserImageWithIdNotFoundException()
    {
        // Arrange
        var userImageId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        _unitOfWork.UserImageRepository
            .CheckImageForUserExists(userId, userImageId, Arg.Any<CancellationToken>())
            .Returns(false);
        
        // Act & Assert
        Assert.ThrowsAsync<UserImageWithIdNotFoundException>(
            () => _sut.RemoveUserImageById(userId, userImageId, CancellationToken.None));
    }
    
    [Test]
    public void SetActiveUserImageForUserId_UserImageNotExist_ThrowsUserImageWithIdNotFoundException()
    {
        // Arrange
        var userImageId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        _unitOfWork.UserImageRepository
            .TryGetImageByIdForUser(userImageId, userId, false, Arg.Any<CancellationToken>())
            .ReturnsNull();
        
        // Act & Assert
        Assert.ThrowsAsync<UserImageWithIdNotFoundException>(
            () => _sut.SetActiveUserImageForUser(userId, userImageId, CancellationToken.None));
    }
}