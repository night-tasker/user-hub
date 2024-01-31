using FluentAssertions;
using NightTasker.UserHub.Core.Application.ApplicationContracts.Repository;
using NightTasker.UserHub.Core.Application.Exceptions.NotFound;
using NightTasker.UserHub.Core.Application.Features.OrganizationUsers.Queries.GetOrganizationUserRole;
using NightTasker.UserHub.Core.Domain.Enums;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace NightTasker.UserHub.Core.Application.UnitTests.Features.OrganizationUsers.Queries;

public class GetOrganizationUserRoleQueryHandlerTests
{
    private IUnitOfWork _unitOfWork = null!;

    [SetUp]
    public void Setup()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
    }

    [Test]
    public async Task Handle_UserIsNotInOrganization_ThrowsOrganizationUserNotFoundException()
    {
        // Arrange
        var query = new GetOrganizationUserRoleQuery(Guid.NewGuid(), Guid.NewGuid());
        var sut = new GetOrganizationUserRoleQueryHandler(_unitOfWork);

        _unitOfWork.OrganizationUserRepository
            .TryGetUserOrganizationRole(query.OrganizationId, query.UserId, Arg.Any<CancellationToken>())
            .ReturnsNull();
        
        // Act
        Func<Task> act = async () => await sut.Handle(query, CancellationToken.None);
        
        // Assert
        await act.Should().ThrowAsync<OrganizationUserNotFoundException>();
    }
    
    [TestCase(OrganizationUserRole.Admin)]
    [TestCase(OrganizationUserRole.Member)]
    public async Task Handle_UserIsInOrganization_ReturnsRole(OrganizationUserRole userRole)
    {
        // Arrange
        var query = new GetOrganizationUserRoleQuery(Guid.NewGuid(), Guid.NewGuid());
        var sut = new GetOrganizationUserRoleQueryHandler(_unitOfWork);

        _unitOfWork.OrganizationUserRepository
            .TryGetUserOrganizationRole(query.OrganizationId, query.UserId, Arg.Any<CancellationToken>())
            .Returns(userRole);
        
        // Act
        var result = await sut.Handle(query, CancellationToken.None);

        // Assert
        result.Should().Be(userRole);
    }
}