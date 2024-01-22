using MediatR;
using NightTasker.UserHub.Core.Application.Features.UserInfos.Models;

namespace NightTasker.UserHub.Core.Application.Features.UserInfos.Queries.GetById;

/// <summary>
/// Запрос для получения информации о пользователе по ИД.
/// </summary>
/// <param name="UserInfoId">ИД сведений о пользователе.</param>
public record GetUserInfoByIdQuery(Guid UserInfoId) : IRequest<UserInfoDto>;