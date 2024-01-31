using MediatR;
using NightTasker.UserHub.Core.Application.Features.UserImages.Models;

namespace NightTasker.UserHub.Core.Application.Features.UserImages.Queries.DownloadByUserId;

public record DownloadUserImageByUserInfoIdQuery(Guid UserInfoId) : IRequest<UserImageWithStreamDto>;