using NightTasker.UserHub.Core.Application.Features.Organizations.Models;

namespace NightTasker.UserHub.Core.Application.Features.Organizations.Services.Contracts;

public interface IOrganizationService
{
    Task<Guid> CreateOrganization(CreateOrganizationDto createOrganizationDto, CancellationToken cancellationToken);
}