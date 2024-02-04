using Microsoft.EntityFrameworkCore;
using NightTasker.UserHub.Core.Domain.Common.Search.Filters;
using NightTasker.UserHub.Core.Domain.Common.Search.Paging;
using NightTasker.UserHub.Core.Domain.Common.Search.Sorters;

namespace NightTasker.UserHub.Core.Domain.Common.Search;

public interface ISearchCriteria<T> where T : class
{
    IFilter<T>? Filter { get; }
    
    ISorter<T>? Sorter { get; }
    
    IPaging<T>? Paging { get; }

    async Task<SearchResult<T>> Apply(IQueryable<T> query, CancellationToken cancellationToken)
    {
        if (Filter != null) 
            query = Filter.Apply(query);
        
        var count = query.Count();
        
        if (Sorter != null) 
            query = Sorter.Apply(query);
        
        if (Paging == null) 
            return new SearchResult<T>(await query.ToListAsync(cancellationToken), 1, count, count);
        
        query = Paging.Apply(query);
        return new SearchResult<T>(await query.ToListAsync(cancellationToken), Paging.Page, Paging.Take, count);
    }
}