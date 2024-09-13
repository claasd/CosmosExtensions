# CosmosExtensions

[![License](https://img.shields.io/badge/license-MIT-blue)](https://github.com/claasd/OtdrReader/blob/main/LICENSE)
[![Nuget](https://img.shields.io/nuget/v/CdIts.CosmosExtensions)](https://www.nuget.org/packages/CdIts.CosmosExtensions/)
[![Nuget](https://img.shields.io/nuget/vpre/CdIts.CosmosExtensions)](https://www.nuget.org/packages/CdIts.CosmosExtensions/)
[![CI](https://github.com/claasd/CosmosExtensions/actions/workflows/build.yml/badge.svg)](https://github.com/claasd/CosmosExtensions/actions/workflows/build.yml)

CosmosExtensions is a library that provides a set of extension methods for the Azure Cosmos DB SDK LINQ methods, especially for async operations without the need to use .HasMoreResults.

CosmosExtensions.Factory implements a factory that creates CosmosClients that use JSON.NET for serialization with customizable JsonSerializer settings and ignoring "required" attributes.
Furthermore, it exposes a CosmosConnector as a way to have a singleton CosmosClient that can be used throughout the application by using dependency injection.

## Examples for CosmosExtensions
```csharp
var item = await container.GetItemLinqQueryable<MyObject>().Where(o => o.Id == "myId").FirstOrDefaultItemAsync();
List<MyObject> allItemsList = await container.GetItemLinqQueryable<MyObject>().ToItemListAsync();
MyObject[] allItemsArray = await container.GetItemLinqQueryable<MyObject>().ToItemArrayAsync();

await foreach(element in container.GetItemLinqQueryable<MyObject>().QueryContainerAsync())
{
    // do something with element
}
```

it is also possible to use all of the above directly with an iterator:

```csharp
string[] distinctValues = await container.GetItemQueryIterator<string>($"select distinct value c.some from c").ToItemArrayAsync();
} 
```

## Available extension methods

* `QueryContainerAsync()` - Executes a query and returns the results as an IAsyncEnumerable.
* `FirstOrDefaultItemAsync()` - Returns the first item if any item is found, returns `default` only if the iterator reports that there are nor results
* `FirstItemAsync()` - Returns the first item if any item is found throws an exception if there are no results
* `ToItemArrayAsync()` - returns all results as an array
* `ToItemListAsync()` - returns all results as a List

## Using CosmosExtensions.Factory with dependecy injection

Add a CosmosConnectorConfig and a CosmosConnector to the service collection:            
```csharp
serviceCollection.AddCosmosConnector(credential, "cosmosSection");
```

You can than use the cosmos connector to access the cosmosClient and the default Database.

at minimum, the config section must provide a "AccountEndpoint" setting. You can provide additional properties as well:
```json
{
  "cosmosSection": {
    "AccountEndpoint": "https://myCosmosAccount.documents.azure.com:443/",
    "DefaultDatabaseName": "db",
    "StoreNullValues" : false
  }
}
```
if you want to use a key instead of a credential, you must provide the "AccountKey" via config and use
```csharp
serviceCollection.AddCosmosConnector("cosmosSection");
```

## License
MIT License

