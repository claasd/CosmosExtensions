using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;

namespace CdIts.CosmosExtensions.Factory;

public class CosmosConnector
{
    public Microsoft.Azure.Cosmos.Database Database { get; }
    public CosmosClient Client { get; }

    public CosmosConnector(IOptions<CosmosConnectorConfig> options)
    {
        try
        {
            var config = options.Value;
            Client = CosmosFactory.GetClient(config.AccountEndpoint, config.AccountKey, config.Credential, config.StoreNullValues);
            Database = Client.GetDatabase(config.DefaultDatabaseName);
        } catch (Exception e)
        {
            throw new Exception("Error creating CosmosConnector", e);
        }
    }
}