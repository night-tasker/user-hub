using NightTasker.UserHub.Core.Application.Exceptions.NotFound;
using NightTasker.UserHub.Core.Application.Features.Organizations.Queries.GetOrganizationById;
using NightTasker.UserHub.Core.Domain.Repositories;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace NightTasker.UserHub.Core.Application.UnitTests.Features.Organizations.Queries;

public class GetOrganizationByIdAsUserQueryHandlerTests
{
    private IUnitOfWork _unitOfWork = null!;
    private GetOrganizationByIdAsUserQueryHandler _sut = null!;

    [SetUp]
    public void Setup()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _sut = new GetOrganizationByIdAsUserQueryHandler(_unitOfWork);
    }

    [Test]
    public void Handle_NoOrganizationWithId_OrganizationNotFoundException()
    {
        // Arrange
        var query = new GetOrganizationByIdAsUserQuery(Guid.NewGuid(), Guid.NewGuid());
        _unitOfWork
            .OrganizationRepository
            .TryGetOrganizationForUser(
                Arg.Any<Guid>(), Arg.Any<Guid>(),  Arg.Any<bool>(), Arg.Any<CancellationToken>())
            .ReturnsNull();
        
        // Act
        var result = _sut.Handle(query, CancellationToken.None);
        
        // Assert
        Assert.ThrowsAsync<OrganizationUserNotFoundException>(async () => await result);
    }
}