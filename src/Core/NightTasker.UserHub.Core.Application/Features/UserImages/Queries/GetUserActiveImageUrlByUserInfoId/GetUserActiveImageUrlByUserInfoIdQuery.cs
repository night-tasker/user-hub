using MediatR;

namespace NightTasker.UserHub.Core.Application.Features.UserImages.Queries.GetUserActiveImageUrlByUserInfoId;

public record GetUserActiveImageUrlByUserInfoIdQuery(Guid UserInfoId) : IRequest<string>;