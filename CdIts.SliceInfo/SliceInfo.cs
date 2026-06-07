using Newtonsoft.Json;

namespace CdIts.Slicing;

public class SliceInfo
{
    [JsonProperty("total")]
    public int Total { get; set; }
    
    [JsonProperty("filtered")]
    public int Filtered { get; set; }
    
    [JsonProperty("offset")]
    public int Offset { get; set; }
    
    [JsonProperty("limit")]
    public int? Limit { get; set; }
    public SliceInfo ToSliceInfo() => new()
    {
        Total = Total,
        Filtered = Filtered,
        Offset = Offset,
        Limit = Limit
    };   
}