using MediatR;
using NightTasker.UserHub.Core.Application.Features.UserImages.Models;

namespace NightTasker.UserHub.Core.Application.Features.UserImages.Queries.GetUserImagesWithUrlByUserInfoId;

public record GetUserImagesWithUrlByUserInfoIdQuery(Guid UserInfoId) : IRequest<IReadOnlyCollection<UserImageWithUrlDto>>;