namespace NightTasker.UserHub.Core.Domain.Common.Search.Paging;

public sealed class Paging<T>(int take, int page) : IPaging<T>
{
    public int Take { get; } = take;

    public int Page { get; } = page;

    public IQueryable<T> Apply(IQueryable<T> query)
    {
        query = query
            .Skip((Page - 1) * Take)
            .Take(Take);

        return query;
    }
}