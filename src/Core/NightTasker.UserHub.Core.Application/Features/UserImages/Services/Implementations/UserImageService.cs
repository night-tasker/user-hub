using MapsterMapper;
using NightTasker.UserHub.Core.Application.ApplicationContracts.Repository;
using NightTasker.UserHub.Core.Application.ApplicationContracts.Services;
using NightTasker.UserHub.Core.Application.Exceptions.NotFound;
using NightTasker.UserHub.Core.Application.Features.UserImages.Models;
using NightTasker.UserHub.Core.Application.Features.UserImages.Services.Contracts;

namespace NightTasker.UserHub.Core.Application.Features.UserImages.Services.Implementations;

/// <inheritdoc />
public class UserImageService(
    IMapper mapper,
    IUnitOfWork unitOfWork, IStorageFileService storageFileService) : IUserImageService
{
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    private readonly IStorageFileService _storageFileService = storageFileService ?? throw new ArgumentNullException(nameof(storageFileService));
    
    /// <inheritdoc />
    public async Task<Guid> CreateUserImage(CreateUserImageDto createUserImageDto, CancellationToken cancellationToken)
    {
        var userImage = _mapper.Map<Domain.Entities.UserImage>(createUserImageDto);
        userImage.IsActive = true;
        await _unitOfWork.UserImageRepository.Add(userImage, cancellationToken);
        await _unitOfWork.UserImageRepository.SetUnActiveImagesForUserInfoIdExcludeOne(
            userImage.UserInfoId, userImage.Id, cancellationToken);
        
        return userImage.Id;
    }

    /// <inheritdoc />
    public async Task<UserImageWithStreamDto> DownloadUserImageByUserInfoId(Guid userInfoId, CancellationToken cancellationToken)
    {
        var userImage = await _unitOfWork.UserImageRepository.TryGetActiveImageByUserInfoId(
            userInfoId, false, cancellationToken);

        if (userImage == null)
        {
            throw new UserImageForUserInfoIdNotFoundException(userInfoId);
        }

        var downloadedFileDto = await _storageFileService.DownloadFile(userImage.Id, cancellationToken);
        var fileStream = new MemoryStream(downloadedFileDto.File);
        var userImageDto = new UserImageWithStreamDto(
            fileStream, userImage.FileName!, userImage.Extension!, userImage.ContentType!);
        return userImageDto;
    }

    /// <inheritdoc />
    public async Task<string> GetUserActiveImageUrlByUserInfoId(Guid userInfoId, CancellationToken cancellationToken)
    {
        var userImage = await _unitOfWork.UserImageRepository.TryGetActiveImageByUserInfoId(
            userInfoId, false, cancellationToken);

        if (userImage == null)
        {
            throw new UserImageForUserInfoIdNotFoundException(userInfoId);
        }
        
        var fileUrl = await _storageFileService.GetFileUrl(userImage.Id, cancellationToken);
        return fileUrl;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyCollection<UserImageWithUrlDto>> GetUserImagesByUserInfoId(
        Guid userInfoId, CancellationToken cancellationToken)
    {
        var userImages = await _unitOfWork.UserImageRepository.GetImageIdsWithActiveByUserInfoId(userInfoId, cancellationToken);
        if (!userImages.Any())
        {
            return Array.Empty<UserImageWithUrlDto>();
        }
        var userImageIds = userImages.Keys.ToHashSet();
        var filesWithUrl = await _storageFileService.GetFilesUrls(userImageIds, cancellationToken);
        var userImagesWithUrl = filesWithUrl
            .Select(fileWithUrl => new UserImageWithUrlDto(fileWithUrl.Key, fileWithUrl.Value, userImages[fileWithUrl.Key]))
            .ToList();

        return userImagesWithUrl;
    }

    /// <inheritdoc />
    public async Task RemoveUserImageById(Guid userImageId, CancellationToken cancellationToken)
    {
        var userImage = await _unitOfWork.UserImageRepository.TryGetImageById(userImageId, false, cancellationToken);
        if (userImage == null)
        {
            throw new UserImageWithIdNotFoundException(userImageId);
        }
        
        await _unitOfWork.UserImageRepository.RemoveUserImageById(userImage.Id, cancellationToken);
        
        await _storageFileService.RemoveFile(userImage.Id, cancellationToken);
    }

    /// <inheritdoc />
    public async Task SetActiveUserImageForUserInfoId(
        Guid userInfoId, Guid userImageId, CancellationToken cancellationToken)
    {
        var userImage = await _unitOfWork.UserImageRepository.TryGetImageById(
            userImageId, true, cancellationToken);
        if (userImage == null)
        {
            throw new UserImageWithIdNotFoundException(userImageId);
        }

        userImage.IsActive = true;
        _unitOfWork.UserImageRepository.Update(userImage);
        
        await _unitOfWork.UserImageRepository.SetUnActiveImagesForUserInfoIdExcludeOne(
            userInfoId, userImageId, cancellationToken);
        
        await _unitOfWork.SaveChanges(cancellationToken);
    }
}