using System.Linq.Expressions;
using Microsoft.Azure.Cosmos.Linq;

namespace CdIts.CosmosExtensions.SliceFilter;

public static class FilterExtensions
{
    public static IQueryable<TSource> OrderBy<TSource, TKey>(this IQueryable<TSource> query, Expression<Func<TSource, TKey>> func, bool descending) =>
        descending ? query.OrderByDescending(func) : query.OrderBy(func);

    public static IQueryable<T> Paginate<T>(this IQueryable<T> query, ICosmosPaginationFilter filter)
    {
        if (filter.Offset > 0)
            query = query.Skip(filter.Offset);
        if (filter.Limit > 0)
            query = query.Take(filter.Limit.Value);
        return query;
    }

    public static async Task<CosmosFilterResult<TResult>> QueryWithFilterAsync<TDb, TResult>(this IQueryable<TDb> query, ICosmosDbFilter<TDb, TResult> filter, CancellationToken cancellationToken = default) where TDb : class
    {
        var total= await query.CountAsync(cancellationToken: cancellationToken);
        query = filter.ApplyFilter(query);
        var filtered = await query.CountAsync(cancellationToken: cancellationToken);
        query = filter.ApplyOrdering(query).Paginate(filter);
        var items = await filter.ApplySelect(query).ToItemListAsync(cancellationToken: cancellationToken);
        return new CosmosFilterResult<TResult>(items, new SliceInfo
        {
            Total = total,
            Filtered = filtered,
            Offset = filter.Offset,
            Limit = filter.Limit
        });
    }
}