using MapsterMapper;
using MediatR;
using NightTasker.UserHub.Core.Application.ApplicationContracts.Repository;
using NightTasker.UserHub.Core.Application.ApplicationContracts.Services;
using NightTasker.UserHub.Core.Application.Exceptions.NotFound;
using NightTasker.UserHub.Core.Application.Features.UserImages.Models;
using NightTasker.UserHub.Core.Application.Features.UserImages.Services.Contracts;
using NightTasker.UserHub.Core.Application.Models.StorageFile;

namespace NightTasker.UserHub.Core.Application.Features.UserImages.Commands.UploadUserImage;

/// <summary>
/// Хэндлер для <see cref="UploadUserImageCommand"/>.
/// </summary>
public class UploadUserImageCommandHandler(
    IStorageFileService storageFileService,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IUserImageService userImageService) : IRequestHandler<UploadUserImageCommand>
{
    private readonly IStorageFileService _storageFileService = storageFileService ?? throw new ArgumentNullException(nameof(storageFileService));
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    private readonly IUserImageService _userImageService = userImageService ?? throw new ArgumentNullException(nameof(userImageService));
    
    public async Task Handle(UploadUserImageCommand request, CancellationToken cancellationToken)
    {
        var userImageId = await CreateUserImage(request, cancellationToken);
        
        await UploadFile(
            fileName: userImageId.ToString(),
            contentType: request.ContentType,
            fileSize: request.FileSize,
            stream: request.Stream, 
            cancellationToken);
        
        await _unitOfWork.SaveChanges(cancellationToken);
    }
    
    private async Task<Guid> CreateUserImage(UploadUserImageCommand request, CancellationToken cancellationToken)
    {
        var userInfo = await _unitOfWork.UserInfoRepository.TryGetById(request.UserId, false, cancellationToken);
        if (userInfo is null)
        {
            throw new UserInfoNotFoundException(request.UserId);
        }

        var createUserImageDto = _mapper.Map<CreateUserImageDto>(request);
        var userImageId = await _userImageService.CreateUserImage(createUserImageDto, cancellationToken);
        return userImageId; 
    }

    private async Task UploadFile(
        string fileName,
        string contentType,
        long fileSize,
        Stream stream,
        CancellationToken cancellationToken)
    {
        await using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream, cancellationToken);
        var uploadUserImageDto = new UploadFileDto(fileName, memoryStream, contentType, fileSize);
        await _storageFileService.UploadFile(uploadUserImageDto, cancellationToken);
    }
}