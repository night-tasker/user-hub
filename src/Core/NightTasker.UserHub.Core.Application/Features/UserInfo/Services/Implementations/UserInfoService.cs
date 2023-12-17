using MapsterMapper;
using Microsoft.Extensions.Logging;
using NightTasker.UserHub.Core.Application.ApplicationContracts.Repository;
using NightTasker.UserHub.Core.Application.Features.UserInfo.Models;
using NightTasker.UserHub.Core.Application.Features.UserInfo.Services.Contracts;

namespace NightTasker.UserHub.Core.Application.Features.UserInfo.Services.Implementations;

/// <inheritdoc />
public class UserInfoService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILogger<UserInfoService> logger) : IUserInfoService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    private readonly ILogger _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    /// <inheritdoc />
    public async Task CreateUserInfoWithSaving(CreateUserInfoDto createUserInfoDto, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[STARTED] Create user info for user {UserId}", createUserInfoDto.Id);
        var userInfo = _mapper.Map<Domain.Entities.UserInfo>(createUserInfoDto);
        await _unitOfWork.UserInfoRepository.Add(userInfo, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);
        _logger.LogInformation("[COMPLETED] Create user info for user {UserId}", createUserInfoDto.Id);
    }
}