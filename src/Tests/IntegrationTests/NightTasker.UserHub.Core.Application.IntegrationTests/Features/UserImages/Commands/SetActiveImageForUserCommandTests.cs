using FluentAssertions;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NightTasker.UserHub.Core.Application.ApplicationContracts.Services;
using NightTasker.UserHub.Core.Application.Exceptions.NotFound;
using NightTasker.UserHub.Core.Application.Features.UserImages.Commands.SetActiveUserImage;
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

public class SetActiveImageForUserCommandTests : ApplicationIntegrationTestsBase
{
    private readonly CancellationTokenSource _cancellationTokenSource;

    public SetActiveImageForUserCommandTests()
    {
        RegisterService(new ServiceForRegister(typeof(IApplicationDataAccessor), serviceProvider => new ApplicationDataAccessor(
            serviceProvider.GetRequiredService<ApplicationDbContext>()), ServiceLifetime.Scoped));
        
        RegisterService(new ServiceForRegister(typeof(IUnitOfWork), 
            serviceProvider => new UnitOfWork(serviceProvider.GetRequiredService<IApplicationDataAccessor>()), ServiceLifetime.Scoped));
        
        RegisterService(new ServiceForRegister(typeof(IUserImageService), serviceProvider => new UserImageService(
            serviceProvider.GetRequiredService<IUnitOfWork>(), 
            Substitute.For<IStorageFileService>()), ServiceLifetime.Scoped));
        
        RegisterService(new ServiceForRegister(typeof(SetActiveImageForUserCommandHandler)));
        
        BuildServiceProvider();
        
        var dbContext = GetService<ApplicationDbContext>();
        dbContext.Database.Migrate();
        _cancellationTokenSource = new CancellationTokenSource();
    }

    [Fact]
    public async Task Handle_ImageIsNotActive_CurrentImageIsActive()
    {
        // Arrange
        var user = SetupUser(Guid.NewGuid());
        var userImageForUpdate = SetupUserImage(user.Id, false);
        
        var dbContext = GetService<ApplicationDbContext>();
        await dbContext.Set<User>().AddAsync(user);
        await dbContext.Set<UserImage>().AddAsync(userImageForUpdate);
        await dbContext.SaveChangesAsync();
        
        var command = new SetActiveImageForUserCommand(user.Id, userImageForUpdate.Id);
        var sut = GetService<SetActiveImageForUserCommandHandler>();
        
        // Act
        await sut.Handle(command, _cancellationTokenSource.Token);
        
        // Assert
        var allImages = await dbContext.Set<UserImage>()
            .Where(x => x.UserId == user.Id)
            .ToListAsync();
        
        var activeImages = allImages.Where(x => x.IsActive).ToList();
        activeImages.Count.Should().Be(1);
        activeImages.First().Id.Should().Be(userImageForUpdate.Id);
    }
    
    [Fact]
    public async Task Handle_ImageNotExist_ThrowsUserImageWithIdNotFoundException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var userImageId = Guid.NewGuid();
        var command = new SetActiveImageForUserCommand(userId, userImageId);
        var sut = GetService<SetActiveImageForUserCommandHandler>();
        
        // Act
        var func = async () => await sut.Handle(command, _cancellationTokenSource.Token);
        
        // Assert
        await func.Should().ThrowAsync<UserImageWithIdNotFoundException>();
    }
    
    private static User SetupUser(Guid userId)
    {
        var user = new User
        {
            Id = userId
        };
        
        return user;
    }

    private static UserImage SetupUserImage(Guid userId, bool isActive)
    {
        var userImage = new UserImage
        {
            UserId = userId,
            IsActive = isActive
        };
        
        return userImage;
    }
}