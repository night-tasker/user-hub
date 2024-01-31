using MapsterMapper;
using NightTasker.UserHub.Core.Application.ApplicationContracts.Repository;
using NightTasker.UserHub.Core.Application.Exceptions.NotFound;
using NightTasker.UserHub.Core.Application.Features.OrganizationUsers.Models;
using NightTasker.UserHub.Core.Application.Features.OrganizationUsers.Services.Implementations;
using NightTasker.UserHub.Core.Domain.Enums;
using NSubstitute;

namespace NightTasker.UserHub.Core.Application.UnitTests.Features.OrganizationUsers.Services;

public class OrganizationUserServiceTests
{
    private IUnitOfWork _unitOfWork = null!;
    private IMapper _mapper = null!;
    private OrganizationUserService _sut = null!;

    [SetUp]
    public void Setup()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _mapper = Substitute.For<IMapper>();
        _sut = new OrganizationUserService(_unitOfWork, _mapper);
    }

    [Test]
    public void CreateOrganizationUserWithOutSaving_UserInfoNotExist_UserInfoNotFoundException()
    {
        // Arrange
        var dto = new CreateOrganizationUserDto(Guid.NewGuid(), Guid.NewGuid(), OrganizationUserRole.Admin);

        _unitOfWork.OrganizationRepository
            .CheckExistsByIdForUser(dto.UserId, dto.OrganizationId, CancellationToken.None)
            .Returns(true);
        
        _unitOfWork.UserInfoRepository
            .CheckExistsById(dto.UserId, CancellationToken.None)
            .Returns(false);
        
        // Act && Assert
        Assert.ThrowsAsync<UserInfoNotFoundException>(() => _sut.CreateOrganizationUser(dto, CancellationToken.None));
    }
    
    [Test]
    public void CreateOrganizationUserWithOutSaving_OrganizationNotExist_OrganizationNotFoundException()
    {
        // Arrange
        var dto = new CreateOrganizationUserDto(Guid.NewGuid(), Guid.NewGuid(), OrganizationUserRole.Admin);

        _unitOfWork.OrganizationRepository
            .CheckExistsByIdForUser(dto.UserId, dto.OrganizationId,  CancellationToken.None)
            .Returns(false);
        
        _unitOfWork.UserInfoRepository
            .CheckExistsById(dto.UserId, CancellationToken.None)
            .Returns(true);
        
        // Act && Assert
        Assert.ThrowsAsync<OrganizationNotFoundException>(() => _sut.CreateOrganizationUser(dto, CancellationToken.None));
    }
}