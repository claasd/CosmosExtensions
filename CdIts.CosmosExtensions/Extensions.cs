using System.Runtime.CompilerServices;
using CdIts.CosmosExtensions.Filter;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

namespace CdIts.CosmosExtensions;

public static class Extensions
{
    public static async IAsyncEnumerable<T> QueryContainerAsync<T>(this FeedIterator<T> iterator,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        while (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync(cancellationToken);
            foreach (var item in response)
                yield return item;
        }
    }

    public static IAsyncEnumerable<T> QueryContainerAsync<T>(this IQueryable<T> query, CancellationToken cancellationToken = default)
        => query.ToFeedIterator().QueryContainerAsync(cancellationToken);

    public static async Task<T?> FirstOrDefaultItemAsync<T>(this FeedIterator<T> iter, CancellationToken cancellationToken = default) =>
        await iter.QueryContainerAsync(cancellationToken).FirstOrDefaultAsync(cancellationToken: cancellationToken);

    public static async Task<T?> FirstOrDefaultItemAsync<T>(this IQueryable<T> query, CancellationToken cancellationToken = default) =>
        await query.QueryContainerAsync(cancellationToken).FirstOrDefaultAsync(cancellationToken: cancellationToken);

    public static async Task<T> FirstItemAsync<T>(this FeedIterator<T> iter, CancellationToken cancellationToken = default) =>
        await iter.QueryContainerAsync(cancellationToken).FirstAsync(cancellationToken: cancellationToken);

    public static async Task<T> FirstItemAsync<T>(this IQueryable<T> query, CancellationToken cancellationToken = default) =>
        await query.QueryContainerAsync(cancellationToken).FirstAsync(cancellationToken: cancellationToken);

    public static async Task<T[]> ToItemArrayAsync<T>(this FeedIterator<T> iter, CancellationToken cancellationToken = default) =>
        await iter.QueryContainerAsync(cancellationToken).ToArrayAsync(cancellationToken: cancellationToken);

    public static async Task<T[]> ToItemArrayAsync<T>(this IQueryable<T> query, CancellationToken cancellationToken = default) =>
        await query.QueryContainerAsync(cancellationToken).ToArrayAsync(cancellationToken: cancellationToken);

    public static async Task<List<T>> ToItemListAsync<T>(this FeedIterator<T> iter, CancellationToken cancellationToken = default) =>
        await iter.QueryContainerAsync(cancellationToken).ToListAsync(cancellationToken: cancellationToken);

    public static async Task<List<T>> ToItemListAsync<T>(this IQueryable<T> query, CancellationToken cancellationToken = default) =>
        await query.QueryContainerAsync(cancellationToken).ToListAsync(cancellationToken: cancellationToken);

    public static IQueryable<T> TakeRange<T>(this IQueryable<T> query, int offset, int? limit)
    {

        if (offset > 0 || limit is > 0)
            query = query.Skip(offset);
        if (limit is > 0)
            query = query.Take(limit.Value);
        return query;
    }
    
    public static IQueryable<T> Filter<T>(this IQueryable<T> query, CosmosCustomFilterBase<T> filterBase)
    {
        return filterBase.ApplyOrdering(filterBase.ApplyFilter(query));
    }
    
}
