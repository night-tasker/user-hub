using MediatR;
using NightTasker.UserHub.Core.Application.Features.UserImage.Models;

namespace NightTasker.UserHub.Core.Application.Features.UserImage.Queries.DownloadByUserId;

/// <summary>
/// Запрос для получения фотографии пользователя по идентификатору.
/// </summary>
/// <param name="UserInfoId">ИД пользователя.</param>
public record DownloadUserImageByUserInfoIdQuery(Guid UserInfoId) : IRequest<UserImageWithStreamDto>;