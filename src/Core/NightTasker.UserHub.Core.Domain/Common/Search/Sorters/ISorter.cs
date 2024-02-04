namespace NightTasker.UserHub.Core.Domain.Common.Search.Sorters;

public interface ISorter<T>
{
    IQueryable<T> Apply(IQueryable<T> query);
}