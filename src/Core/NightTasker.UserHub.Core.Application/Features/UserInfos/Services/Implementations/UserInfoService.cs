using MapsterMapper;
using NightTasker.UserHub.Core.Application.ApplicationContracts.Repository;
using NightTasker.UserHub.Core.Application.Exceptions.NotFound;
using NightTasker.UserHub.Core.Application.Features.UserInfos.Models;
using NightTasker.UserHub.Core.Application.Features.UserInfos.Services.Contracts;
using NightTasker.UserHub.Core.Domain.Entities;

namespace NightTasker.UserHub.Core.Application.Features.UserInfos.Services.Implementations;

public class UserInfoService(
    IUnitOfWork unitOfWork,
    IMapper mapper) : IUserInfoService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    public async Task CreateUserInfo(CreateUserInfoDto createUserInfoDto, CancellationToken cancellationToken)
    {
        var userInfo = _mapper.Map<UserInfo>(createUserInfoDto);
        await _unitOfWork.UserInfoRepository.Add(userInfo, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);
    }

    public async Task UpdateUserInfo(UpdateUserInfoDto updateUserInfoDto, CancellationToken cancellationToken)
    {
        var userInfo = await GetUserInfoById(updateUserInfoDto.Id, true, cancellationToken);
        _mapper.Map(updateUserInfoDto, userInfo);
        _unitOfWork.UserInfoRepository.Update(userInfo);
        await _unitOfWork.SaveChanges(cancellationToken);
    }

    private async Task<UserInfo> GetUserInfoById(
        Guid userInfoId, bool trackChanges, CancellationToken cancellationToken)
    {
        var userInfo = await _unitOfWork.UserInfoRepository.TryGetById(userInfoId, trackChanges, cancellationToken);
        if (userInfo is null)
        {
            throw new UserInfoNotFoundException(userInfoId);
        }
        
        return userInfo;
    }
}