using Azure.Core;
using Newtonsoft.Json;

namespace CdIts.CosmosExtensions.Factory;

public class CosmosConnectorConfig
{
    public TokenCredential? Credential { get; set; }
    public string AccountEndpoint { get; set; } = "";

    [Obsolete("Use AccountEndpoint instead")]
    public string Endpoint
    {
        get => AccountEndpoint;
        set => AccountEndpoint = value;
    }
    public string? AccountKey { get; set; }
    public string DefaultDatabaseName { get; set; } = "db";
    
    [Obsolete("Use DefaultDatabaseName instead")]
    public string Database
    {
        get => DefaultDatabaseName;
        set => DefaultDatabaseName = value;
    }
    public bool StoreNullValues { get; set; } = false;
    public JsonSerializerSettings? Settings { get; set; }
}