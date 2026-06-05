namespace CdIts.CosmosExtensions.SliceFilter;

public interface ICosmosPaginationFilter
{
    int Offset { get; }
    int? Limit { get; }
}