using Azure.Core;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;

namespace CdIts.CosmosExtensions.Factory;

/// <summary>
/// create cosmosClients with specified settings, either based on a key or based on a token credential
/// all cosmos client use JSON.NET for serialization, and add a contract resolver that ignores "required" properties
/// Additional JsonSerializer settings can be passed
/// </summary>
public static class CosmosFactory
{
    public static CosmosClient GetClient(string endpoint, string key) => GetClient(endpoint, key, null);
    public static CosmosClient GetClient(string endpoint, TokenCredential credential) => GetClient(endpoint, null, credential);

    public static CosmosClient GetClient(string endpoint, string? key, TokenCredential? credential) =>
        GetClient(endpoint, key, credential, NullValueHandling.Ignore);

    public static CosmosClient GetClient(string endpoint, string? key, TokenCredential? credential, bool storeNullValues) =>
        GetClient(endpoint, key, credential, storeNullValues ? NullValueHandling.Include : NullValueHandling.Ignore);

    public static CosmosClient GetClient(string endpoint, string? key, TokenCredential? credential, NullValueHandling nullValueHandling)
        => GetClient(endpoint, key, credential, new JsonSerializerSettings { NullValueHandling = nullValueHandling });

    public static CosmosClient GetClient(string endpoint, string? key, TokenCredential? credential, JsonSerializerSettings settings)
    {
        var cosmosConfig = new CosmosClientOptions()
        {
            Serializer = new IgnoreRequiredSerializer(settings)
        };
        return key != null
            ? new CosmosClient(endpoint, key, cosmosConfig)
            : new CosmosClient(endpoint, credential, cosmosConfig);
    }
}