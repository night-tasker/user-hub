using NightTasker.UserHub.Core.Application.Features.OrganizationUsers.Models;

namespace NightTasker.UserHub.Core.Application.Features.OrganizationUsers.Services.Contracts;

public interface IOrganizationUserService
{
    Task CreateOrganizationUser(
        CreateOrganizationUserDto organizationUserDto, CancellationToken cancellationToken);
}