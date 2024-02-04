using NightTasker.UserHub.Core.Application.Common.Enums;
using NightTasker.UserHub.Core.Domain.Common.Search.Sorters;
using NightTasker.UserHub.Core.Domain.Entities;

namespace NightTasker.UserHub.Core.Application.Features.Organizations.Models.Search;

public class OrganizationsSorter(IDictionary<string, SortDirection> sortFields) : ISorter<Organization>
{
    public IDictionary<string, SortDirection> SortFields { get; } = sortFields;

    public IQueryable<Organization> Apply(IQueryable<Organization> query)
    {
        foreach (var (key, value) in SortFields)
        {
            if (String.Compare(nameof(Organization.Name), key, StringComparison.OrdinalIgnoreCase) == 0)
                query = SortByName(query, value);

            if (String.Compare(nameof(Organization.Description), key, StringComparison.OrdinalIgnoreCase) == 0)
                query = SortByDescription(query, value);
            
            if (String.Compare(nameof(OrganizationDto.CreatedAt), key, StringComparison.OrdinalIgnoreCase) == 0)
                query = SortByCreatedAt(query, value);
        }
        return query;
    }
    
    private static IQueryable<Organization> SortByName(IQueryable<Organization> query, SortDirection direction)
    {
        if (direction == SortDirection.Ascending)
            query = query
                .OrderBy(x => x.Name);
        else
            query = query
                .OrderByDescending(x => x.Name);
        
        return query;
    }

    private static IQueryable<Organization> SortByDescription(IQueryable<Organization> query, SortDirection direction)
    {
        if (direction == SortDirection.Ascending)
            query = query
                .OrderBy(x => x.Description);
        else
            query = query
                .OrderByDescending(x => x.Description);
        return query;
    }
    
    private static IQueryable<Organization> SortByCreatedAt(IQueryable<Organization> query, SortDirection value)
    {
        if (value == SortDirection.Ascending)
            query = query
                .OrderBy(x => x.CreatedDateTimeOffset);
        else
            query = query
                .OrderByDescending(x => x.CreatedDateTimeOffset);
        return query;
    }
}