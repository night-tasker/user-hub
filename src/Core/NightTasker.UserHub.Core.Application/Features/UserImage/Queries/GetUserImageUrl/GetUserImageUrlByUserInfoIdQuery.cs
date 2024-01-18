using MediatR;

namespace NightTasker.UserHub.Core.Application.Features.UserImage.Queries.GetUserImageUrl;

/// <summary>
/// Запрос для получения ссылки на фотографию пользователя.
/// </summary>
/// <param name="UserInfoId">ИД пользователя.</param>
public record GetUserImageUrlByUserInfoIdQuery(Guid UserInfoId) : IRequest<string>;