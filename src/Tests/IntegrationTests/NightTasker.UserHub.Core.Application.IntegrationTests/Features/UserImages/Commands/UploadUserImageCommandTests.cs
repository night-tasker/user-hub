using Bogus;
using FluentAssertions;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NightTasker.UserHub.Core.Application.ApplicationContracts.Services;
using NightTasker.UserHub.Core.Application.Features.UserImages.Commands.UploadUserImage;
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

public class UploadUserImageCommandTests : ApplicationIntegrationTestsBase
{
    private readonly Faker _faker;
    private readonly CancellationTokenSource _cancellationTokenSource;

    public UploadUserImageCommandTests(TestNpgSql testNpgSql) : base(testNpgSql)
    {
        RegisterService(new ServiceForRegister(typeof(IApplicationDataAccessor), serviceProvider => new ApplicationDataAccessor(
            serviceProvider.GetRequiredService<ApplicationDbContext>()), ServiceLifetime.Scoped));
        
        RegisterService(new ServiceForRegister(typeof(IUnitOfWork), 
            serviceProvider => new UnitOfWork(serviceProvider.GetRequiredService<IApplicationDataAccessor>()), ServiceLifetime.Scoped));
        
        RegisterService(new ServiceForRegister(
            typeof(IStorageFileService), _ => Substitute.For<IStorageFileService>(), ServiceLifetime.Scoped));
        
        RegisterService(new ServiceForRegister(typeof(IUserImageService), serviceProvider => new UserImageService(
            serviceProvider.GetRequiredService<IUnitOfWork>(), 
            Substitute.For<IStorageFileService>()), ServiceLifetime.Scoped));
        
        RegisterService(new ServiceForRegister(typeof(UploadUserImageCommandHandler)));
        
        BuildServiceProvider();

        PrepareDatabase();
        
        _faker = new Faker();
        _cancellationTokenSource = new CancellationTokenSource();
    }

    [Fact]
    public async Task Handle_UploadUserImage()
    {
        // Arrange
        var user = SetupUser(Guid.NewGuid());
        var command = new UploadUserImageCommand(
            user.Id,
            new MemoryStream(), 
            _faker.Random.AlphaNumeric(8), 
            ".jpg", 
            "image/jpeg", 
            50);
        
        var dbContext = GetService<ApplicationDbContext>();
        await dbContext.Set<User>().AddAsync(user);
        await dbContext.SaveChangesAsync();
        var sut = GetService<UploadUserImageCommandHandler>();
        
        // Act
        await sut.Handle(command, _cancellationTokenSource.Token);
        
        // Assert
        var userImage = await dbContext.Set<UserImage>()
            .Where(x => x.UserId == user.Id)
            .SingleOrDefaultAsync();
        
        userImage.Should().NotBeNull();
        user.Id.Should().Be(userImage!.UserId);
        userImage.IsActive.Should().BeTrue();
        userImage.FileName.Should().Be(command.FileName);
        userImage.Extension.Should().Be(command.FileExtension);
        userImage.ContentType.Should().Be(command.ContentType);
        userImage.FileSize.Should().Be(command.FileSize);
    }
    
    private static User SetupUser(Guid userId)
    {
        return User.CreateInstance(userId);
    }
}