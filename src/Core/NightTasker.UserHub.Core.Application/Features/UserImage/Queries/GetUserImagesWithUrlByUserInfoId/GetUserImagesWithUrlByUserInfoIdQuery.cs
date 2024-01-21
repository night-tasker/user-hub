using MediatR;
using NightTasker.UserHub.Core.Application.Features.UserImage.Models;

namespace NightTasker.UserHub.Core.Application.Features.UserImage.Queries.GetUserImagesWithUrlByUserInfoId;

/// <summary>
/// Получить ссылки на фотографии пользователя.
/// </summary>
/// <param name="UserInfoId">ИД пользователя.</param>
public record GetUserImagesWithUrlByUserInfoIdQuery(Guid UserInfoId) : IRequest<IReadOnlyCollection<UserImageWithUrlDto>>;