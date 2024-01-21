using Mapster;
using NightTasker.UserHub.Core.Application.Features.UserImage.Commands.UploadUserImage;
using NightTasker.UserHub.Core.Application.Features.UserImage.Models;
using NightTasker.UserHub.Core.Application.Models.StorageFile;

namespace NightTasker.UserHub.Presentation.WebApi.Profiles.UserImage;

public class UserImageProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<CreateUserImageDto, Core.Domain.Entities.UserImage>();
        
        config.ForType<UploadUserImageCommand, CreateUserImageDto>()
            .Map(dest => dest.UserInfoId, src => src.UserId);
    }
}