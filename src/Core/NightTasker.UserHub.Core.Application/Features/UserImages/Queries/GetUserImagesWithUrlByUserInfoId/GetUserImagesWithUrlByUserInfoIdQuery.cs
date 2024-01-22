using MediatR;
using NightTasker.UserHub.Core.Application.Features.UserImages.Models;

namespace NightTasker.UserHub.Core.Application.Features.UserImages.Queries.GetUserImagesWithUrlByUserInfoId;

/// <summary>
/// Получить ссылки на фотографии пользователя.
/// </summary>
/// <param name="UserInfoId">ИД пользователя.</param>
public record GetUserImagesWithUrlByUserInfoIdQuery(Guid UserInfoId) : IRequest<IReadOnlyCollection<UserImageWithUrlDto>>;