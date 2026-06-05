using CdIts.Slicing;

namespace CdIts.CosmosExtensions.SliceFilter;

public class CosmosFilterResult<T>(List<T> items, SliceInfo sliceInfo)
{
    public List<T> Items { get; } = items;
    public SliceInfo SliceInfo { get; } = sliceInfo;
}