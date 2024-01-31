using Bogus;
using FluentAssertions;
using MapsterMapper;
using NightTasker.UserHub.Core.Application.ApplicationContracts.Repository;
using NightTasker.UserHub.Core.Application.Exceptions.NotFound;
using NightTasker.UserHub.Core.Application.Features.UserInfos.Models;
using NightTasker.UserHub.Core.Application.Features.UserInfos.Services.Implementations;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace NightTasker.UserHub.Core.Application.UnitTests.Features.UserInfos.Services;

public class UserInfoServiceTests
{
    private IUnitOfWork _unitOfWork = null!;
    private UserInfoService _sut = null!;
    private Faker _faker = null!;

    [SetUp]
    public void Setup()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _sut = new UserInfoService(_unitOfWork, Substitute.For<IMapper>());
        _faker = new Faker();
    }

    [Test]
    public async Task UpdateUserInfo_UserInfoNotExist_ThrowsUserInfoNotFoundException()
    {
        // Arrange
        var updateUserInfoDto = SetUpdateUserInfoDto();
        _unitOfWork.UserInfoRepository.TryGetById(updateUserInfoDto.Id, true, Arg.Any<CancellationToken>())
            .ReturnsNull();
        
        // Act
        var act = async () => await _sut.UpdateUserInfo(updateUserInfoDto, CancellationToken.None);
        
        // Assert
        await act.Should().ThrowAsync<UserInfoNotFoundException>();
    }

    private UpdateUserInfoDto SetUpdateUserInfoDto()
    {
        var firstName = _faker.Random.AlphaNumeric(8);
        var middleName = _faker.Random.AlphaNumeric(8);
        var lastName = _faker.Random.AlphaNumeric(8);
        var updateUserInfoDto = new UpdateUserInfoDto(Guid.NewGuid(), firstName, middleName, lastName);
        
        return updateUserInfoDto;
    }
}