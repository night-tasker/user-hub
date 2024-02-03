using Bogus;
using FluentAssertions;
using NightTasker.UserHub.Core.Application.Exceptions.NotFound;
using NightTasker.UserHub.Core.Application.Features.Users.Models;
using NightTasker.UserHub.Core.Application.Features.Users.Services.Implementations;
using NightTasker.UserHub.Core.Domain.Repositories;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace NightTasker.UserHub.Core.Application.UnitTests.Features.Users.Services;

public class UserServiceTests
{
    private IUnitOfWork _unitOfWork = null!;
    private UserService _sut = null!;
    private Faker _faker = null!;

    [SetUp]
    public void Setup()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _sut = new UserService(_unitOfWork);
        _faker = new Faker();
    }

    [Test]
    public async Task UpdateUser_UserNotExist_ThrowsUserNotFoundException()
    {
        // Arrange
        var updateUserDto = SetUpdateUserDto();
        _unitOfWork.UserRepository.TryGetById(updateUserDto.Id, true, Arg.Any<CancellationToken>())
            .ReturnsNull();
        
        // Act
        var act = async () => await _sut.UpdateUser(updateUserDto, CancellationToken.None);
        
        // Assert
        await act.Should().ThrowAsync<UserNotFoundException>();
    }

    private UpdateUserDto SetUpdateUserDto()
    {
        var firstName = _faker.Random.AlphaNumeric(8);
        var middleName = _faker.Random.AlphaNumeric(8);
        var lastName = _faker.Random.AlphaNumeric(8);
        var updateUserDto = new UpdateUserDto(Guid.NewGuid(), firstName, middleName, lastName);
        
        return updateUserDto;
    }
}