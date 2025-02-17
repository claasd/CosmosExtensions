using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;

namespace CdIts.CosmosExtensions.Factory;

public class CosmosConnector
{
    public Database Database { get; protected set; }
    public CosmosClient Client { get; protected set; }

    protected CosmosConnector()
    {
        // allow overwriting to use their own options
    }

    protected CosmosConnector(CosmosClient client, string dbName)
    {
        Client = client;
        Database = client.GetDatabase(dbName);
    }

    public CosmosConnector(IOptions<CosmosConnectorConfig> options)
    {
        try
        {
            var config = options.Value;
            Client = config.Settings != null
                ? CosmosFactory.GetClient(config.AccountEndpoint, config.AccountKey, config.Credential, config.Settings)
                : CosmosFactory.GetClient(config.AccountEndpoint, config.AccountKey, config.Credential, config.StoreNullValues);
            Database = Client.GetDatabase(config.DefaultDatabaseName);
        }
        catch (Exception e)
        {
            throw new Exception("Error creating CosmosConnector", e);
        }
    }
}