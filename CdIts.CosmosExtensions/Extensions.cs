using System.Runtime.CompilerServices;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

namespace CdIts.CosmosExtensions;

public static class Extensions
{
    public static async IAsyncEnumerable<T> QueryContainerAsync<T>(this IQueryable<T> query, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var iterator = query.ToFeedIterator();
        while (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync(cancellationToken);
            foreach (var item in response)
                yield return item;
        }   
    }

    public static async Task<T?> FirstOrDefaultItemAsync<T>(this IQueryable<T> query, CancellationToken cancellationToken = default) =>
        await query.QueryContainerAsync(cancellationToken).FirstOrDefaultAsync(cancellationToken: cancellationToken);
    
    public static async Task<T> FirstItemAsync<T>(this IQueryable<T> query, CancellationToken cancellationToken = default) =>
        await query.QueryContainerAsync(cancellationToken).FirstAsync(cancellationToken: cancellationToken);
    
    public static async Task<T[]> ToItemArrayAsync<T>(this IQueryable<T> query, CancellationToken cancellationToken = default) =>
        await query.QueryContainerAsync(cancellationToken).ToArrayAsync(cancellationToken: cancellationToken);
    
    public static async Task<List<T>> ToItemListAsync<T>(this IQueryable<T> query, CancellationToken cancellationToken = default) =>
        await query.QueryContainerAsync(cancellationToken).ToListAsync(cancellationToken: cancellationToken);
    
    
}