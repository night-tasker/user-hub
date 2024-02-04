using MediatR;
using NightTasker.UserHub.Core.Application.Features.Organizations.Models;
using NightTasker.UserHub.Core.Domain.Common.Search;
using NightTasker.UserHub.Core.Domain.Entities;
using NightTasker.UserHub.Core.Domain.Repositories;

namespace NightTasker.UserHub.Core.Application.Features.Organizations.Queries.SearchOrganizationsAsUser;

internal class SearchOrganizationsAsUserQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<SearchOrganizationsAsUserQuery, SearchResult<OrganizationDto>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

    public async Task<SearchResult<OrganizationDto>> Handle(SearchOrganizationsAsUserQuery request, CancellationToken cancellationToken)
    {
        var organizationsSearchResult = await _unitOfWork.OrganizationRepository
            .SearchOrganizationsForUser(request.UserId, request.SearchCriteria, cancellationToken);
        return ConvertEntitySearchResultToDto(organizationsSearchResult);
    }

    private SearchResult<OrganizationDto> ConvertEntitySearchResultToDto(SearchResult<Organization> searchResult)
    {
        var organizationsDto = OrganizationDto.FromEntities(searchResult.Items);
        return new SearchResult<OrganizationDto>(
            organizationsDto, searchResult.Page, searchResult.Take, searchResult.TotalCount);
    }
}