using MediatR;
using NightTasker.UserHub.Core.Application.Exceptions.NotFound;
using NightTasker.UserHub.Core.Application.Features.UserInfos.Models;
using NightTasker.UserHub.Core.Domain.Entities;
using NightTasker.UserHub.Core.Domain.Repositories;

namespace NightTasker.UserHub.Core.Application.Features.UserInfos.Queries.GetById;

internal class GetUserInfoByIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetUserInfoByIdQuery, UserInfoDto>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

    public async Task<UserInfoDto> Handle(GetUserInfoByIdQuery request, CancellationToken cancellationToken)
    {
        var userInfo = await GetUserInfoById(request.UserInfoId, cancellationToken);
        var userInfoDto = UserInfoDto.FromEntity(userInfo);
        return userInfoDto;
    }

    private async Task<User> GetUserInfoById(Guid userInfoId, CancellationToken cancellationToken)
    {
        var userInfo = await _unitOfWork.UserInfoRepository.TryGetById(userInfoId, false, cancellationToken);
        if (userInfo is null)
        {
            throw new UserInfoNotFoundException(userInfoId);
        }
        
        return userInfo;
    }
}