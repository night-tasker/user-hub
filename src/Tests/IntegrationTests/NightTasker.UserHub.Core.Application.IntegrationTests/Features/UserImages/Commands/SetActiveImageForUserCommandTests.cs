﻿using FluentAssertions;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NightTasker.UserHub.Core.Application.ApplicationContracts.Repository;
using NightTasker.UserHub.Core.Application.ApplicationContracts.Services;
using NightTasker.UserHub.Core.Application.Exceptions.NotFound;
using NightTasker.UserHub.Core.Application.Features.UserImages.Commands.SetActiveUserImage;
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

public class SetActiveImageForUserCommandTests : ApplicationIntegrationTestsBase
{
    private readonly ISender _sender;
    
    public SetActiveImageForUserCommandTests()
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
    public async Task Handle_ImageIsNotActive_CurrentImageIsActive()
    {
        // Arrange
        var userInfo = SetupUserInfo(Guid.NewGuid());
        var userImageForUpdate = SetupUserImage(userInfo.Id, false);
        
        var dbContext = GetService<ApplicationDbContext>();
        await dbContext.Set<UserInfo>().AddAsync(userInfo);
        await dbContext.Set<UserImage>().AddAsync(userImageForUpdate);
        await dbContext.SaveChangesAsync();
        
        var command = new SetActiveImageForUserCommand(userInfo.Id, userImageForUpdate.Id);
        
        // Act
        await _sender.Send(command);
        
        // Assert
        var allImages = await dbContext.Set<UserImage>()
            .Where(x => x.UserInfoId == userInfo.Id)
            .ToListAsync();
        
        var activeImages = allImages.Where(x => x.IsActive).ToList();
        activeImages.Count.Should().Be(1);
        activeImages.First().Id.Should().Be(userImageForUpdate.Id);
    }
    
    [Fact]
    public async Task Handle_ImageNotExist_ThrowsUserImageWithIdNotFoundException()
    {
        // Arrange
        var userInfoId = Guid.NewGuid();
        var userImageId = Guid.NewGuid();
        var command = new SetActiveImageForUserCommand(userInfoId, userImageId);
        
        // Act
        var func = async () => await _sender.Send(command);
        
        // Assert
        await func.Should().ThrowAsync<UserImageWithIdNotFoundException>();
    }
    
    private static UserInfo SetupUserInfo(Guid userId)
    {
        var userInfo = new UserInfo
        {
            Id = userId
        };
        
        return userInfo;
    }

    private static UserImage SetupUserImage(Guid userId, bool isActive)
    {
        var userImage = new UserImage
        {
            UserInfoId = userId,
            IsActive = isActive
        };
        
        return userImage;
    }
}