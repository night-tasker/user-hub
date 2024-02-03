using MediatR;

namespace NightTasker.UserHub.Core.Application.Features.UserImages.Queries.GetUserActiveImageUrlByUserId;

public record GetUserActiveImageUrlByUserIdQuery(Guid UserId) : IRequest<string>;