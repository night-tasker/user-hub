using MediatR;
using NightTasker.UserHub.Core.Application.Features.Organizations.Models;
using NightTasker.UserHub.Core.Application.Features.Organizations.Models.Search;
using NightTasker.UserHub.Core.Domain.Common.Search;

namespace NightTasker.UserHub.Core.Application.Features.Organizations.Queries.SearchOrganizationsAsUser;

public record SearchOrganizationsAsUserQuery(
    Guid UserId, OrganizationsSearchCriteria SearchCriteria) : IRequest<SearchResult<OrganizationDto>>;