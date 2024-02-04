using NightTasker.UserHub.Core.Application.Features.Organizations.Models.Search;
using NightTasker.UserHub.Core.Domain.Common.Search.Paging;

namespace NightTasker.UserHub.Presentation.WebApi.Requests.Organization;

public record SearchOrganizationsRequest(
    OrganizationsFilter Filter,
    OrganizationsSorter Sorter,
    Paging<Core.Domain.Entities.Organization> Paging);