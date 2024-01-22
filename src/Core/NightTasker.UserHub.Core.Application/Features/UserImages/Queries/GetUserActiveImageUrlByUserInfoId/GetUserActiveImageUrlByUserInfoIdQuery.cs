using MediatR;

namespace NightTasker.UserHub.Core.Application.Features.UserImages.Queries.GetUserActiveImageUrlByUserInfoId;

/// <summary>
/// Запрос для получения ссылки на активную фотографию пользователя.
/// </summary>
/// <param name="UserInfoId">ИД пользователя.</param>
public record GetUserActiveImageUrlByUserInfoIdQuery(Guid UserInfoId) : IRequest<string>;