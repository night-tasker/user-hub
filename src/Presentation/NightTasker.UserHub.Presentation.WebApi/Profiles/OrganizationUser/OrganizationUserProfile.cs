using Mapster;
using NightTasker.UserHub.Core.Application.Features.OrganizationUsers.Models;

namespace NightTasker.UserHub.Presentation.WebApi.Profiles.OrganizationUser;

public class OrganizationUserProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<CreateOrganizationUserDto, Core.Domain.Entities.OrganizationUser>();
    }
}