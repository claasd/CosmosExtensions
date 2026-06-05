namespace CdIts.CosmosExtensions.SliceFilter;

public class SliceInfo
{
    public int Total { get; set; }
    public int Filtered { get; set; }
    public int Offset { get; set; }
    public int? Limit { get; set; }
}