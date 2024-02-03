using MediatR;
using NightTasker.UserHub.Core.Application.Features.UserImages.Models;

namespace NightTasker.UserHub.Core.Application.Features.UserImages.Queries.DownloadByUserId;

public record DownloadUserImageByUserIdQuery(Guid UserId) : IRequest<UserImageWithStreamDto>;