using MapsterMapper;
using MediatR;
using NightTasker.UserHub.Core.Application.ApplicationContracts.Repository;
using NightTasker.UserHub.Core.Application.Exceptions.NotFound;
using NightTasker.UserHub.Core.Application.Features.UserInfos.Models;
using NightTasker.UserHub.Core.Domain.Entities;

namespace NightTasker.UserHub.Core.Application.Features.UserInfos.Queries.GetById;

internal class GetUserInfoByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetUserInfoByIdQuery, UserInfoDto>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    public async Task<UserInfoDto> Handle(GetUserInfoByIdQuery request, CancellationToken cancellationToken)
    {
        var userInfo = await GetUserInfoById(request.UserInfoId, cancellationToken);
        var userInfoDto = _mapper.Map<UserInfoDto>(userInfo);
        return userInfoDto;
    }

    private async Task<UserInfo> GetUserInfoById(Guid userInfoId, CancellationToken cancellationToken)
    {
        var userInfo = await _unitOfWork.UserInfoRepository.TryGetById(userInfoId, false, cancellationToken);
        if (userInfo is null)
        {
            throw new UserInfoNotFoundException(userInfoId);
        }
        
        return userInfo;
    }
}