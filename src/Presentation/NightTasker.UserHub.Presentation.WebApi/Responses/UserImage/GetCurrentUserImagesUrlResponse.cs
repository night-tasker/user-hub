using NightTasker.UserHub.Core.Application.Features.UserImages.Models;

namespace NightTasker.UserHub.Presentation.WebApi.Responses.UserImage;

public record GetCurrentUserImagesUrlResponse(IReadOnlyCollection<UserImageWithUrlDto> Images);