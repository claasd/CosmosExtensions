namespace CdIts.CosmosExtensions.Filter;

public abstract class CosmosCustomFilterBase<T>
{
    public virtual IQueryable<T> ApplyFilter(IQueryable<T> query) => query;
    public virtual IQueryable<T> ApplyOrdering(IQueryable<T> query) => query;
}