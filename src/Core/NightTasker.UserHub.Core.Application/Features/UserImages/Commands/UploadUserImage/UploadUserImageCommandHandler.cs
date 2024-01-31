using MapsterMapper;
using MediatR;
using NightTasker.UserHub.Core.Application.ApplicationContracts.Services;
using NightTasker.UserHub.Core.Application.Features.UserImages.Models;
using NightTasker.UserHub.Core.Application.Features.UserImages.Services.Contracts;
using NightTasker.UserHub.Core.Application.Models.StorageFile;

namespace NightTasker.UserHub.Core.Application.Features.UserImages.Commands.UploadUserImage;

internal class UploadUserImageCommandHandler(
    IStorageFileService storageFileService,
    IMapper mapper,
    IUserImageService userImageService) : IRequestHandler<UploadUserImageCommand>
{
    private readonly IStorageFileService _storageFileService = storageFileService ?? throw new ArgumentNullException(nameof(storageFileService));
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
    }
    
    private async Task<Guid> CreateUserImage(UploadUserImageCommand request, CancellationToken cancellationToken)
    {
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