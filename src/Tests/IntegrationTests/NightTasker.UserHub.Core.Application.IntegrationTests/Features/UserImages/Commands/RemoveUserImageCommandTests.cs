using FluentAssertions;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NightTasker.UserHub.Core.Application.ApplicationContracts.Repository;
using NightTasker.UserHub.Core.Application.ApplicationContracts.Services;
using NightTasker.UserHub.Core.Application.Exceptions.NotFound;
using NightTasker.UserHub.Core.Application.Features.UserImages.Commands.RemoveUserImage;
using NightTasker.UserHub.Core.Application.Features.UserImages.Services.Contracts;
using NightTasker.UserHub.Core.Application.Features.UserImages.Services.Implementations;
using NightTasker.UserHub.Core.Domain.Entities;
using NightTasker.UserHub.Infrastructure.Persistence;
using NightTasker.UserHub.Infrastructure.Persistence.Contracts;
using NightTasker.UserHub.Infrastructure.Persistence.Repository.Common;
using NightTasker.UserHub.IntegrationTests.Framework;
using NightTasker.UserHub.Presentation.WebApi.Implementations;
using NSubstitute;
using Xunit;

namespace NightTasker.UserHub.Core.Application.IntegrationTests.Features.UserImages.Commands;

public class RemoveUserImageCommandTests : ApplicationIntegrationTestsBase
{
    private readonly ISender _sender;
    
    public RemoveUserImageCommandTests()
    {
        RegisterService(new ServiceForRegister(typeof(IApplicationDbAccessor), serviceProvider => new ApplicationDbAccessor(
            serviceProvider.GetRequiredService<ApplicationDbContext>()), ServiceLifetime.Scoped));
        
        RegisterService(new ServiceForRegister(typeof(IUnitOfWork), 
            serviceProvider => new UnitOfWork(serviceProvider.GetRequiredService<IApplicationDbAccessor>()), ServiceLifetime.Scoped));
        
        RegisterService(new ServiceForRegister(typeof(IUserImageService), serviceProvider => new UserImageService(
                serviceProvider.GetRequiredService<IMapper>(), 
                serviceProvider.GetRequiredService<IUnitOfWork>(), 
                Substitute.For<IStorageFileService>()), ServiceLifetime.Scoped));
        
        BuildServiceProvider();
        
        var dbContext = GetService<ApplicationDbContext>();
        dbContext.Database.Migrate();
        _sender = GetService<ISender>();
    }

    [Fact]
    public async Task Handle_UserImageNotExist_ThrowsUserImageNotFoundException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var userImageId = Guid.NewGuid();

        // Assert & Act
        await Assert.ThrowsAsync<UserImageWithIdNotFoundException>(
            async () => await _sender.Send(new RemoveUserImageCommand(userId, userImageId)));
    }

    [Fact]
    public async Task Handle_UserImageExists_UserImageRemoved()
    {
        // Arrange
        var dbContext = GetService<ApplicationDbContext>();
        var userId = Guid.NewGuid();
        var user = SetupUserInfo(userId);
        var userImage = SetupUserImage(userId);
        await dbContext.Set<UserInfo>().AddAsync(user);
        await dbContext.Set<UserImage>().AddAsync(userImage);
        await dbContext.SaveChangesAsync();
        var command = new RemoveUserImageCommand(userId, userImage.Id);
        
        // Act
        await _sender.Send(command);
        
        // Assert
        var updatedImage = await dbContext.Set<UserImage>()
            .FirstOrDefaultAsync(x => x.Id == userImage.Id);
        
        updatedImage.Should().BeNull();
    }
    
    private static UserInfo SetupUserInfo(Guid userId)
    {
        var userInfo = new UserInfo
        {
            Id = userId
        };
        
        return userInfo;
    }

    private static UserImage SetupUserImage(Guid userId)
    {
        var userImage = new UserImage
        {
            UserInfoId = userId
        };
        
        return userImage;
    }
}