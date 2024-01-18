using MapsterMapper;
using NightTasker.UserHub.Core.Application.ApplicationContracts.Repository;
using NightTasker.UserHub.Core.Application.ApplicationContracts.Services;
using NightTasker.UserHub.Core.Application.Exceptions.NotFound;
using NightTasker.UserHub.Core.Application.Features.UserImage.Models;
using NightTasker.UserHub.Core.Application.Features.UserImage.Services.Contracts;

namespace NightTasker.UserHub.Core.Application.Features.UserImage.Services.Implementations;

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
    public async Task<UserImageDto> DownloadUserImageByUserInfoId(Guid userInfoId, CancellationToken cancellationToken)
    {
        var userImage = await _unitOfWork.UserImageRepository.TryGetByUserInfoId(
            userInfoId, false, cancellationToken);

        if (userImage == null)
        {
            throw new UserImageForUserInfoIdNotFoundException(userInfoId);
        }

        var downloadedFileDto = await _storageFileService.DownloadFile(userImage.Id, cancellationToken);
        var fileStream = new MemoryStream(downloadedFileDto.File);
        var userImageDto = new UserImageDto(
            fileStream, userImage.FileName!, userImage.Extension!, userImage.ContentType!);
        return userImageDto;
    }

    /// <inheritdoc />
    public async Task<string> GetUserImageUrlByUserInfoId(Guid userInfoId, CancellationToken cancellationToken)
    {
        var userImage = await _unitOfWork.UserImageRepository.TryGetByUserInfoId(
            userInfoId, false, cancellationToken);

        if (userImage == null)
        {
            throw new UserImageForUserInfoIdNotFoundException(userInfoId);
        }
        
        var fileUrl = await _storageFileService.GetFileUrl(userImage.Id, cancellationToken);
        return fileUrl;
    }
}