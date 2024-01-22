using MapsterMapper;
using NightTasker.UserHub.Core.Application.ApplicationContracts.Repository;
using NightTasker.UserHub.Core.Application.Exceptions.NotFound;
using NightTasker.UserHub.Core.Application.Features.UserInfos.Models;
using NightTasker.UserHub.Core.Application.Features.UserInfos.Services.Contracts;

namespace NightTasker.UserHub.Core.Application.Features.UserInfos.Services.Implementations;

/// <inheritdoc />
public class UserInfoService(
    IUnitOfWork unitOfWork,
    IMapper mapper) : IUserInfoService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    /// <inheritdoc />
    public async Task<UserInfoDto> GetUserInfoById(Guid userInfoId, CancellationToken cancellationToken)
    {
        var userInfo = await _unitOfWork.UserInfoRepository.TryGetById(userInfoId, false, cancellationToken);
        if (userInfo is null)
        {
            throw new UserInfoNotFoundException(userInfoId);
        }
        
        return _mapper.Map<UserInfoDto>(userInfo);
    }

    /// <inheritdoc />
    public async Task CreateUserInfoWithSaving(CreateUserInfoDto createUserInfoDto, CancellationToken cancellationToken)
    {
        var userInfo = _mapper.Map<Domain.Entities.UserInfo>(createUserInfoDto);
        await _unitOfWork.UserInfoRepository.Add(userInfo, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);
    }

    /// <inheritdoc />
    public async Task UpdateUserInfoWithSaving(UpdateUserInfoDto updateUserInfoDto, CancellationToken cancellationToken)
    {
        var userInfo = await _unitOfWork.UserInfoRepository.TryGetById(updateUserInfoDto.Id, true, cancellationToken);
        if (userInfo is null)
        {
            throw new UserInfoNotFoundException(updateUserInfoDto.Id);
        }

        _mapper.Map(updateUserInfoDto, userInfo);
        _unitOfWork.UserInfoRepository.Update(userInfo);
        await _unitOfWork.SaveChanges(cancellationToken);
    }
}