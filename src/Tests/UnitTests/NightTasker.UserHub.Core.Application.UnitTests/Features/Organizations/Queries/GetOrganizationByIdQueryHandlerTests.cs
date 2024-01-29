using MapsterMapper;
using NightTasker.UserHub.Core.Application.ApplicationContracts.Repository;
using NightTasker.UserHub.Core.Application.Exceptions.NotFound;
using NightTasker.UserHub.Core.Application.Features.Organizations.Queries.GetOrganizationById;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace NightTasker.UserHub.Core.Application.UnitTests.Features.Organizations.Queries;

public class GetOrganizationByIdQueryHandlerTests
{
    private IUnitOfWork _unitOfWork = null!;
    private GetOrganizationByIdQueryHandler _sut = null!;
    private IMapper _mapper = null!;

    [SetUp]
    public void Setup()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _mapper = Substitute.For<IMapper>();
        _sut = new GetOrganizationByIdQueryHandler(_unitOfWork, _mapper);
    }

    [Test]
    public void Handle_NoOrganizationWithId_OrganizationNotFoundException()
    {
        // Arrange
        var query = new GetOrganizationByIdQuery(Guid.NewGuid());
        _unitOfWork
            .OrganizationRepository
            .TryGetOrganizationWithInfo(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .ReturnsNull();
        
        // Act
        var result = _sut.Handle(query, CancellationToken.None);
        
        // Assert
        Assert.ThrowsAsync<OrganizationNotFoundException>(async () => await result);
    }
}