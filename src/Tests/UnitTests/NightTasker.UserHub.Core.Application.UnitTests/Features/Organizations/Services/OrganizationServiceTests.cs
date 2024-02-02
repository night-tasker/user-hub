using Bogus;
using FluentAssertions;
using MapsterMapper;
using NightTasker.UserHub.Core.Application.ApplicationContracts.Repository;
using NightTasker.UserHub.Core.Application.Exceptions.NotFound;
using NightTasker.UserHub.Core.Application.Features.Organizations.Models;
using NightTasker.UserHub.Core.Application.Features.Organizations.Services.Implementations;
using NSubstitute;

namespace NightTasker.UserHub.Core.Application.UnitTests.Features.Organizations.Services;

public class OrganizationServiceTests
{
    private IUnitOfWork _unitOfWork = null!;
    private OrganizationService _sut = null!;
    private Faker _faker = null!;

    [SetUp]
    public void Setup()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _sut = new OrganizationService(_unitOfWork, Substitute.For<IMapper>());
        _faker = new Faker();
    }

    [Test]
    public async Task UpdateOrganization_OrganizationDoesNotExist_ReturnsOrganizationNotFoundException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var organizationId = Guid.NewGuid();
        _unitOfWork.OrganizationRepository
            .CheckExistsByIdForUser(userId, organizationId, Arg.Any<CancellationToken>())
            .Returns(false);
        var updateOrganizationDto = new UpdateOrganizationDto(
            _faker.Random.AlphaNumeric(8), _faker.Random.AlphaNumeric(24));
        
        // Act
        var func = async () => await _sut.UpdateOrganizationAsUser(userId, organizationId, updateOrganizationDto, CancellationToken.None);
        
        // Assert
        await func.Should().ThrowAsync<OrganizationNotFoundException>();
    }
}