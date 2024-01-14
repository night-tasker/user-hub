using MapsterMapper;
using NightTasker.UserHub.Core.Application.ApplicationContracts.Repository;
using NightTasker.UserHub.Core.Application.Features.UserImage.Models;
using NightTasker.UserHub.Core.Application.Features.UserImage.Services.Contracts;

namespace NightTasker.UserHub.Core.Application.Features.UserImage.Services.Implementations;

/// <inheritdoc />
public class UserImageService(
    IMapper mapper,
    IUnitOfWork unitOfWork) : IUserImageService
{
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    
    /// <inheritdoc />
    public async Task<Guid> CreateUserImage(CreateUserImageDto createUserImageDto, CancellationToken cancellationToken)
    {
        var userImage = _mapper.Map<Domain.Entities.UserImage>(createUserImageDto);
        await _unitOfWork.UserImageRepository.Add(userImage, cancellationToken);
        return userImage.Id;
    }
    
    /// <inheritdoc />
    public async Task<Guid> CreateUserImageWithSaving(CreateUserImageDto createUserImageDto, CancellationToken cancellationToken)
    {
        var userImageId = await CreateUserImage(createUserImageDto, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);
        return userImageId;
    }
}