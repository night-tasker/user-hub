using Mapster;
using NightTasker.UserHub.Core.Application.Features.UserImages.Commands.UploadUserImage;
using NightTasker.UserHub.Core.Application.Features.UserImages.Models;

namespace NightTasker.UserHub.Presentation.WebApi.Profiles.UserImage;

public class UserImageProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<CreateUserImageDto, Core.Domain.Entities.UserImage>();
        
        config.ForType<UploadUserImageCommand, CreateUserImageDto>()
            .Map(dest => dest.UserInfoId, src => src.UserId)
            .Map(dest => dest.Extension, src => src.FileExtension);
    }
}