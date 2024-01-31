using MapsterMapper;
using NightTasker.UserHub.Core.Application.ApplicationContracts.Repository;
using NightTasker.UserHub.Core.Application.ApplicationContracts.Services;
using NightTasker.UserHub.Core.Application.Exceptions.NotFound;
using NightTasker.UserHub.Core.Application.Features.UserImages.Models;
using NightTasker.UserHub.Core.Application.Features.UserImages.Services.Contracts;
using NightTasker.UserHub.Core.Domain.Entities;

namespace NightTasker.UserHub.Core.Application.Features.UserImages.Services.Implementations;

public class UserImageService(
    IMapper mapper,
    IUnitOfWork unitOfWork,
    IStorageFileService storageFileService) : IUserImageService
{
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    private readonly IStorageFileService _storageFileService =
        storageFileService ?? throw new ArgumentNullException(nameof(storageFileService));
    
    public async Task<Guid> CreateUserImage(
        CreateUserImageDto createUserImageDto, CancellationToken cancellationToken)
    {
        await ValidateUserInfoExists(createUserImageDto.UserInfoId, cancellationToken);
            
        var userImage = _mapper.Map<UserImage>(createUserImageDto);
        userImage.IsActive = true;
        await _unitOfWork.UserImageRepository.Add(userImage, cancellationToken);
        await _unitOfWork.UserImageRepository.SetUnActiveImagesForUserInfoIdExcludeOne(
            userImage.UserInfoId, userImage.Id, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);

        return userImage.Id;
    }

    public async Task<UserImageWithStreamDto> DownloadActiveUserImageByUserInfoId(Guid userInfoId,
        CancellationToken cancellationToken)
    {
        var userImage = await GetActiveImageByUserInfoId(
            userInfoId, false, cancellationToken);

        var downloadedFileDto = await _storageFileService.DownloadFile(userImage.Id, cancellationToken);
        var fileStream = new MemoryStream(downloadedFileDto.File);
        var userImageDto = new UserImageWithStreamDto(
            fileStream, userImage.FileName!, userImage.Extension!, userImage.ContentType!);
        return userImageDto;
    }

    public async Task<string> GetUserActiveImageUrlByUserInfoId(Guid userInfoId, CancellationToken cancellationToken)
    {
        var userImage = await GetActiveImageByUserInfoId(
            userInfoId, false, cancellationToken);

        var fileUrl = await _storageFileService.GetFileUrl(userImage.Id, cancellationToken);
        return fileUrl;
    }

    public async Task<IReadOnlyCollection<UserImageWithUrlDto>> GetUserImagesByUserInfoId(
        Guid userInfoId, CancellationToken cancellationToken)
    {
        var userImages =
            await _unitOfWork.UserImageRepository.GetImageIdsWithActiveByUserInfoId(userInfoId, cancellationToken);
        
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

    public async Task SetActiveUserImageForUserInfoId(
        Guid userInfoId, Guid userImageId, CancellationToken cancellationToken)
    {
        var userImage = await GetImageByIdForUser(userInfoId, userImageId, true, cancellationToken);

        userImage.IsActive = true;
        _unitOfWork.UserImageRepository.Update(userImage);

        await _unitOfWork.UserImageRepository.SetUnActiveImagesForUserInfoIdExcludeOne(
            userInfoId, userImageId, cancellationToken);

        await _unitOfWork.SaveChanges(cancellationToken);
    }

    private async Task<UserImage> GetImageByIdForUser(
        Guid userInfoId, Guid userImageId, bool trackChanges, CancellationToken cancellationToken)
    {
        var userImage = await _unitOfWork.UserImageRepository.TryGetImageByIdForUser(
            userImageId, userInfoId, trackChanges, cancellationToken);

        if (userImage == null)
        {
            throw new UserImageWithIdNotFoundException(userImageId);
        }

        return userImage;
    }

    private async Task<UserImage> GetActiveImageByUserInfoId(
        Guid userInfoId, bool trackChanges, CancellationToken cancellationToken)
    {
        var userImage = await _unitOfWork.UserImageRepository.TryGetActiveImageByUserInfoId(
            userInfoId, trackChanges, cancellationToken);

        if (userImage == null)
        {
            throw new ActiveUserImageForUserInfoIdNotFoundException(userInfoId);
        }
        
        return userImage;
    }

    private async Task ValidateUserInfoExists(Guid userInfoId, CancellationToken cancellationToken)
    {
        var userInfo = await _unitOfWork.UserInfoRepository.TryGetById(userInfoId, false, cancellationToken);
        if (userInfo is null)
        {
            throw new UserInfoNotFoundException(userInfoId);
        }
    }

    private async Task ValidateUserImageExists(Guid userInfoId, Guid userImageId, CancellationToken cancellationToken)
    {
        var userImageExists =
            await _unitOfWork.UserImageRepository.CheckImageForUserExists(userInfoId, userImageId, cancellationToken);

        if (!userImageExists)
        {
            throw new UserImageWithIdNotFoundException(userImageId);
        }
    }
}