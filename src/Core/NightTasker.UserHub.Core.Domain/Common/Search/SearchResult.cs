namespace NightTasker.UserHub.Core.Domain.Common.Search;

public record SearchResult<T>(IReadOnlyCollection<T> Items, int Page, int Take, int TotalCount);