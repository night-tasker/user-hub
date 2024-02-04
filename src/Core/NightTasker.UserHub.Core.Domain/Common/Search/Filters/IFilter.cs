namespace NightTasker.UserHub.Core.Domain.Common.Search.Filters;

public interface IFilter<T> where T : class
{
    IQueryable<T> Apply(IQueryable<T> query);
}