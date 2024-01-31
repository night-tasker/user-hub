using MediatR;
using NightTasker.UserHub.Core.Application.Features.UserInfos.Models;

namespace NightTasker.UserHub.Core.Application.Features.UserInfos.Queries.GetById;

public record GetUserInfoByIdQuery(Guid UserInfoId) : IRequest<UserInfoDto>;