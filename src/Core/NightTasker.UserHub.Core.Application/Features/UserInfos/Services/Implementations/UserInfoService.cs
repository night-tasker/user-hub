using NightTasker.UserHub.Core.Application.Exceptions.NotFound;
using NightTasker.UserHub.Core.Application.Features.UserInfos.Models;
using NightTasker.UserHub.Core.Application.Features.UserInfos.Services.Contracts;
using NightTasker.UserHub.Core.Domain.Entities;
using NightTasker.UserHub.Core.Domain.Repositories;

namespace NightTasker.UserHub.Core.Application.Features.UserInfos.Services.Implementations;

public class UserInfoService(
    IUnitOfWork unitOfWork) : IUserInfoService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

    public async Task CreateUserInfo(CreateUserInfoDto createUserInfoDto, CancellationToken cancellationToken)
    {
        var userInfo = createUserInfoDto.ToEntity();
        await _unitOfWork.UserInfoRepository.Add(userInfo, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);
    }

    public async Task UpdateUserInfo(UpdateUserInfoDto updateUserInfoDto, CancellationToken cancellationToken)
    {
        var userInfo = await GetUserInfoById(updateUserInfoDto.Id, true, cancellationToken);
        userInfo = updateUserInfoDto.MapFieldsToEntity(userInfo);
        _unitOfWork.UserInfoRepository.Update(userInfo);
        await _unitOfWork.SaveChanges(cancellationToken);
    }

    private async Task<User> GetUserInfoById(
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