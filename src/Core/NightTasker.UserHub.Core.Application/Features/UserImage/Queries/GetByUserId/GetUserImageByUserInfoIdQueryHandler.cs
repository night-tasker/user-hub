using MediatR;
using NightTasker.UserHub.Core.Application.ApplicationContracts.Repository;
using NightTasker.UserHub.Core.Application.ApplicationContracts.Services;
using NightTasker.UserHub.Core.Application.Exceptions.NotFound;
using NightTasker.UserHub.Core.Application.Features.UserImage.Models;

namespace NightTasker.UserHub.Core.Application.Features.UserImage.Queries.GetByUserId;

/// <summary>
/// Хэндлер для <see cref="GetUserImageByUserInfoIdQuery"/>
/// </summary>
internal class GetUserImageByUserInfoIdQueryHandler(IStorageFileService storageFileService, IUnitOfWork unitOfWork) : IRequestHandler<GetUserImageByUserInfoIdQuery, UserImageDto>
{
    private readonly IStorageFileService _storageFileService = storageFileService ?? throw new ArgumentNullException(nameof(storageFileService));
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    
    public async Task<UserImageDto> Handle(GetUserImageByUserInfoIdQuery request, CancellationToken cancellationToken)
    {
        var userImage = await _unitOfWork.UserImageRepository.TryGetByUserInfoId(
            request.UserInfoId, false, cancellationToken);

        if (userImage == null)
        {
            throw new UserImageForUserInfoIdNotFoundException(request.UserInfoId);
        }

        var downloadedFileDto = await _storageFileService.DownloadFile(userImage.Id, cancellationToken);
        var fileStream = new MemoryStream(downloadedFileDto.File);
        var userImageDto = new UserImageDto(
            fileStream, userImage.FileName!, userImage.Extension!, userImage.ContentType!);
        return userImageDto;
    }
}