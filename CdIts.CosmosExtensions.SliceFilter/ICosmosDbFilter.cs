namespace CdIts.CosmosExtensions.SliceFilter;

    public interface ICosmosDbFilter<TDb, out TTarget> : ICosmosPaginationFilter
    {
        IQueryable<TDb> ApplyFilter(IQueryable<TDb> query);
        IQueryable<TDb> ApplyOrdering(IQueryable<TDb> query);
        IQueryable<TTarget> ApplySelect(IQueryable<TDb> query);
        
    }
