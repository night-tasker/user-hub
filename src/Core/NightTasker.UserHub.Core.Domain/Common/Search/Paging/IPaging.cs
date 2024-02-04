namespace NightTasker.UserHub.Core.Domain.Common.Search.Paging;

public interface IPaging<T>
{
    int Take { get; }
    
    int Page { get; }

    IQueryable<T> Apply(IQueryable<T> query);
}