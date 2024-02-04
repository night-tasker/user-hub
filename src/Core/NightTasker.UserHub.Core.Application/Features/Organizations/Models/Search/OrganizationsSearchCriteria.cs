using NightTasker.UserHub.Core.Domain.Common.Search;
using NightTasker.UserHub.Core.Domain.Common.Search.Filters;
using NightTasker.UserHub.Core.Domain.Common.Search.Paging;
using NightTasker.UserHub.Core.Domain.Common.Search.Sorters;
using NightTasker.UserHub.Core.Domain.Entities;

namespace NightTasker.UserHub.Core.Application.Features.Organizations.Models.Search;

public class OrganizationsSearchCriteria(
    OrganizationsFilter? filter = null,
    OrganizationsSorter? sorter = null,
    Paging<Organization>? paging = null)
    : ISearchCriteria<Organization>
{
    public IFilter<Organization>? Filter { get; } = filter;

    public ISorter<Organization>? Sorter { get; } = sorter;

    public IPaging<Organization>? Paging { get; } = paging;
}