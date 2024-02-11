using NightTasker.UserHub.Core.Application.ApplicationContracts.Services;
using NightTasker.UserHub.Core.Application.Exceptions.NotFound;
using NightTasker.UserHub.Core.Application.Features.UserImages.Models;
using NightTasker.UserHub.Core.Application.Features.UserImages.Services.Contracts;
using NightTasker.UserHub.Core.Domain.Entities;
using NightTasker.UserHub.Core.Domain.Repositories;

namespace NightTasker.UserHub.Core.Application.Features.UserImages.Services.Implementations;

public class UserImageService(
    IUnitOfWork unitOfWork,
    IStorageFileService storageFileService) : IUserImageService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    private readonly IStorageFileService _storageFileService =
        storageFileService ?? throw new ArgumentNullException(nameof(storageFileService));
    
    public async Task<Guid> CreateUserImage(
        CreateUserImageDto createUserImageDto, CancellationToken cancellationToken)
    {
        await ValidateUserExists(createUserImageDto.UserId, cancellationToken);
            
        var userImage = createUserImageDto.ToEntity();
        userImage.SetActive();
        
        await _unitOfWork.UserImageRepository.Add(userImage, cancellationToken);
        await _unitOfWork.UserImageRepository.SetUnActiveImagesForUserIdExcludeOne(
            userImage.UserId, userImage.Id, cancellationToken);
        return userImage.Id;
    }

    public async Task<UserImageWithStreamDto> DownloadActiveUserImageByUserId(Guid userId,
        CancellationToken cancellationToken)
    {
        var userImage = await GetActiveImageByUserId(
            userId, false, cancellationToken);

        var downloadedFileDto = await _storageFileService.DownloadFile(userImage.Id, cancellationToken);
        var fileStream = new MemoryStream(downloadedFileDto.File);
        var userImageDto = new UserImageWithStreamDto(
            fileStream, userImage.FileName!, userImage.Extension!, userImage.ContentType!);
        return userImageDto;
    }

    public async Task<string> GetUserActiveImageUrlByUserId(Guid userId, CancellationToken cancellationToken)
    {
        var userImage = await GetActiveImageByUserId(
            userId, false, cancellationToken);

        var fileUrl = await _storageFileService.GetFileUrl(userImage.Id, cancellationToken);
        return fileUrl;
    }

    public async Task<IReadOnlyCollection<UserImageWithUrlDto>> GetUserImagesByUserId(
        Guid userId, CancellationToken cancellationToken)
    {
        var userImages =
            await _unitOfWork.UserImageRepository.GetImageIdsWithActiveByUserId(userId, cancellationToken);
        
        if (!userImages.Any())
        {
            return Array.Empty<UserImageWithUrlDto>();
        }

        var userImageIds = userImages.Keys.ToHashSet();
        var filesWithUrl = await _storageFileService.GetFilesUrls(userImageIds, cancellationToken);
        var userImagesWithUrl = filesWithUrl
            .Select(fileWithUrl =>
                new UserImageWithUrlDto(fileWithUrl.Key, fileWithUrl.Value, userImages[fileWithUrl.Key]))
            .ToList();

        return userImagesWithUrl;
    }

    public async Task RemoveUserImageById(Guid userId, Guid userImageId, CancellationToken cancellationToken)
    {
        await ValidateUserImageExists(userId, userImageId, cancellationToken);

        await _unitOfWork.UserImageRepository.RemoveUserImageById(userImageId, cancellationToken);
        await _storageFileService.RemoveFile(userImageId, cancellationToken);
    }

    public async Task SetActiveUserImageForUser(
        Guid userId, Guid userImageId, CancellationToken cancellationToken)
    {
        var userImage = await GetImageByIdForUser(userId, userImageId, true, cancellationToken);

        userImage.SetActive();
        await _unitOfWork.UserImageRepository.SetUnActiveImagesForUserIdExcludeOne(
            userId, userImageId, cancellationToken);
    }

    private async Task<UserImage> GetImageByIdForUser(
        Guid userId, Guid userImageId, bool trackChanges, CancellationToken cancellationToken)
    {
        var userImage = await _unitOfWork.UserImageRepository.TryGetImageByIdForUser(
            userImageId, userId, trackChanges, cancellationToken);

        if (userImage == null)
        {
            throw new UserImageWithIdNotFoundException(userImageId);
        }

        return userImage;
    }

    private async Task<UserImage> GetActiveImageByUserId(
        Guid userId, bool trackChanges, CancellationToken cancellationToken)
    {
        var userImage = await _unitOfWork.UserImageRepository.TryGetActiveImageByUserId(
            userId, trackChanges, cancellationToken);

        if (userImage == null)
        {
            throw new ActiveUserImageForUserIdNotFoundException(userId);
        }
        
        return userImage;
    }

    private async Task ValidateUserExists(Guid userId, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.UserRepository.TryGetById(userId, false, cancellationToken);
        if (user is null)
        {
            throw new UserNotFoundException(userId);
        }
    }

    private async Task ValidateUserImageExists(Guid userId, Guid userImageId, CancellationToken cancellationToken)
    {
        var userImageExists =
            await _unitOfWork.UserImageRepository.CheckImageForUserExists(userId, userImageId, cancellationToken);

        if (!userImageExists)
        {
            throw new UserImageWithIdNotFoundException(userImageId);
        }
    }
}