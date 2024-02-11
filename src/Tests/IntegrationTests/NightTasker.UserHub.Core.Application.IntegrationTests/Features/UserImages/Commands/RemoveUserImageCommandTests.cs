using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NightTasker.UserHub.Core.Application.ApplicationContracts.Services;
using NightTasker.UserHub.Core.Application.Exceptions.NotFound;
using NightTasker.UserHub.Core.Application.Features.UserImages.Commands.RemoveUserImage;
using NightTasker.UserHub.Core.Application.Features.UserImages.Services.Contracts;
using NightTasker.UserHub.Core.Application.Features.UserImages.Services.Implementations;
using NightTasker.UserHub.Core.Domain.Entities;
using NightTasker.UserHub.Core.Domain.Repositories;
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
    private readonly CancellationTokenSource _cancellationTokenSource;

    public RemoveUserImageCommandTests(TestNpgSql testNpgSql) : base(testNpgSql)
    {
        RegisterService(new ServiceForRegister(typeof(IApplicationDataAccessor), serviceProvider => new ApplicationDataAccessor(
            serviceProvider.GetRequiredService<ApplicationDbContext>()), ServiceLifetime.Scoped));
        
        RegisterService(new ServiceForRegister(typeof(IUnitOfWork), 
            serviceProvider => new UnitOfWork(serviceProvider.GetRequiredService<IApplicationDataAccessor>()), ServiceLifetime.Scoped));
        
        RegisterService(new ServiceForRegister(typeof(IUserImageService), serviceProvider => new UserImageService(
                serviceProvider.GetRequiredService<IUnitOfWork>(), 
                Substitute.For<IStorageFileService>()), ServiceLifetime.Scoped));
        
        RegisterService(new ServiceForRegister(typeof(RemoveUserImageCommandHandler)));
        
        BuildServiceProvider();
        
        PrepareDatabase();
        
        _cancellationTokenSource = new CancellationTokenSource();
    }

    [Fact]
    public async Task Handle_UserImageNotExist_ThrowsUserImageNotFoundException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var userImageId = Guid.NewGuid();
        var sut = GetService<RemoveUserImageCommandHandler>();

        // Assert & Act
        await Assert.ThrowsAsync<UserImageWithIdNotFoundException>(
            async () => await sut.Handle(
                new RemoveUserImageCommand(userId, userImageId), _cancellationTokenSource.Token));
    }

    [Fact]
    public async Task Handle_UserImageExists_UserImageRemoved()
    {
        // Arrange
        var dbContext = GetService<ApplicationDbContext>();
        var userId = Guid.NewGuid();
        var user = SetupUser(userId);
        var userImage = SetupUserImage(userId);
        await dbContext.Set<User>().AddAsync(user);
        await dbContext.Set<UserImage>().AddAsync(userImage);
        await dbContext.SaveChangesAsync();
        var command = new RemoveUserImageCommand(userId, userImage.Id);
        var sut = GetService<RemoveUserImageCommandHandler>();
        
        // Act
        await sut.Handle(command, _cancellationTokenSource.Token);
        
        // Assert
        var updatedImage = await dbContext.Set<UserImage>()
            .FirstOrDefaultAsync(x => x.Id == userImage.Id);
        
        updatedImage.Should().BeNull();
    }
    
    private static User SetupUser(Guid userId)
    {
        return User.CreateInstance(userId); 
    }

    private static UserImage SetupUserImage(Guid userId)
    {
        var userImage = UserImage.CreateInstance(
            id: Guid.NewGuid(), userId: userId, null, null, null, 0);
        
        return userImage;
    }
}