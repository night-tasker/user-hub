using NightTasker.UserHub.Core.Domain.Common.Search.Filters;
using NightTasker.UserHub.Core.Domain.Entities;

namespace NightTasker.UserHub.Core.Application.Features.Organizations.Models.Search;

public class OrganizationsFilter : IFilter<Organization>
{
    public string? Name { get; set; }
    
    public IQueryable<Organization> Apply(IQueryable<Organization> query)
    {
        if (!string.IsNullOrEmpty(Name))
        {
            query = query.Where(x => x.Name != null && x.Name.Contains(Name));
        }
        
        return query;
    }
}