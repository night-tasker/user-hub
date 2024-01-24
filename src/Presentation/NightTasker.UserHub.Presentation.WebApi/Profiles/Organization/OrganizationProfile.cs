using Mapster;
using NightTasker.UserHub.Core.Application.Features.Organizations.Commands.CreateOrganization;
using NightTasker.UserHub.Core.Application.Features.Organizations.Models;
using NightTasker.UserHub.Presentation.WebApi.Requests.Organization;

namespace NightTasker.UserHub.Presentation.WebApi.Profiles.Organization;

public class OrganizationProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<CreateOrganizationRequest, CreateOrganizationAsUserCommand>();
        config.ForType<CreateOrganizationAsUserCommand, CreateOrganizationDto>();
        config.ForType<CreateOrganizationDto, Core.Domain.Entities.Organization>();
        
        config.ForType<Core.Domain.Entities.Organization, OrganizationDto>();
    }
}