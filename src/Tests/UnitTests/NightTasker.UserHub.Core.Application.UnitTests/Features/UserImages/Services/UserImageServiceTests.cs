using Bogus;
using FluentAssertions;
using MapsterMapper;
using NightTasker.UserHub.Core.Application.ApplicationContracts.Repository;
using NightTasker.UserHub.Core.Application.ApplicationContracts.Services;
using NightTasker.UserHub.Core.Application.Exceptions.NotFound;
using NightTasker.UserHub.Core.Application.Features.UserImages.Models;
using NightTasker.UserHub.Core.Application.Features.UserImages.Services.Implementations;
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
        _sut = new UserImageService(
            Substitute.For<IMapper>(), _unitOfWork, Substitute.For<IStorageFileService>());
        _faker = new Faker();
    }

    [Test]
    public void CreateUserImage_UserInfoNotExist_ThrowsUserInfoNotFoundException()
    {
        // Arrange
        var createUserImageDto = new CreateUserImageDto(
            Guid.NewGuid(), _faker.Random.AlphaNumeric(8), "png", "image/png", 100);
       
        _unitOfWork.UserInfoRepository
            .TryGetById(createUserImageDto.UserInfoId, false, Arg.Any<CancellationToken>())
            .ReturnsNull();

        // Act & Assert
        Assert.ThrowsAsync<UserInfoNotFoundException>(
            () => _sut.CreateUserImage(createUserImageDto, CancellationToken.None));
    }

    [Test]
    public void DownloadActiveUserImageByUserInfoId_ActiveImageNotExist_ThrowsActiveUserImageForUserInfoIdNotFoundException()
    {
        // Arrange
        var userInfoId = Guid.NewGuid();
        _unitOfWork.UserImageRepository
            .TryGetActiveImageByUserInfoId(userInfoId, false, Arg.Any<CancellationToken>())
            .ReturnsNull();
        
        // Act & Assert
        Assert.ThrowsAsync<ActiveUserImageForUserInfoIdNotFoundException>(
            () => _sut.DownloadActiveUserImageByUserInfoId(userInfoId, CancellationToken.None));
    }
    
    [Test]
    public void GetUserActiveImageUrlByUserInfoId_ActiveImageNotExist_ThrowsActiveUserImageForUserInfoIdNotFoundException()
    {
        // Arrange
        var userInfoId = Guid.NewGuid();
        _unitOfWork.UserImageRepository
            .TryGetActiveImageByUserInfoId(userInfoId, false, Arg.Any<CancellationToken>())
            .ReturnsNull();
        
        // Act & Assert
        Assert.ThrowsAsync<ActiveUserImageForUserInfoIdNotFoundException>(
            () => _sut.DownloadActiveUserImageByUserInfoId(userInfoId, CancellationToken.None));
    }

    [Test]
    public async Task GetUserImagesByUserInfoId_UserImagesNotExist_EmptyCollection()
    {
        // Arrange
        var userInfoId = Guid.NewGuid();
        _unitOfWork.UserImageRepository
            .GetImageIdsWithActiveByUserInfoId(userInfoId, Arg.Any<CancellationToken>())
            .Returns(new Dictionary<Guid, bool>());
        
        // Act
        var result = await _sut.GetUserImagesByUserInfoId(userInfoId, CancellationToken.None);
        
        // Assert
        result.Should().BeEmpty();
    }

    [Test]
    public void RemoveUserImageById_UserImageNotExist_ThrowsUserImageWithIdNotFoundException()
    {
        // Arrange
        var userImageId = Guid.NewGuid();
        var userInfoId = Guid.NewGuid();
        _unitOfWork.UserImageRepository
            .CheckImageForUserExists(userInfoId, userImageId, Arg.Any<CancellationToken>())
            .Returns(false);
        
        // Act & Assert
        Assert.ThrowsAsync<UserImageWithIdNotFoundException>(
            () => _sut.RemoveUserImageById(userInfoId, userImageId, CancellationToken.None));
    }
    
    [Test]
    public void SetActiveUserImageForUserInfoId_UserImageNotExist_ThrowsUserImageWithIdNotFoundException()
    {
        // Arrange
        var userImageId = Guid.NewGuid();
        var userInfoId = Guid.NewGuid();
        _unitOfWork.UserImageRepository
            .TryGetImageByIdForUser(userImageId, userInfoId, false, Arg.Any<CancellationToken>())
            .ReturnsNull();
        
        // Act & Assert
        Assert.ThrowsAsync<UserImageWithIdNotFoundException>(
            () => _sut.SetActiveUserImageForUserInfoId(userInfoId, userImageId, CancellationToken.None));
    }
}