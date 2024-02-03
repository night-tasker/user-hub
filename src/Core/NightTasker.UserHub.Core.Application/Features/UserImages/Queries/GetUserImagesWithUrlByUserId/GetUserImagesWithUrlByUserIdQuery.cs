using MediatR;
using NightTasker.UserHub.Core.Application.Features.UserImages.Models;

namespace NightTasker.UserHub.Core.Application.Features.UserImages.Queries.GetUserImagesWithUrlByUserId;

public record GetUserImagesWithUrlByUserIdQuery(Guid UserId) : IRequest<IReadOnlyCollection<UserImageWithUrlDto>>;