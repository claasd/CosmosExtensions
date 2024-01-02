# CosmosExtensions

[![License](https://img.shields.io/badge/license-MIT-blue)](https://github.com/claasd/OtdrReader/blob/main/LICENSE)
[![Nuget](https://img.shields.io/nuget/v/CdIts.CosmosExtensions)](https://www.nuget.org/packages/CdIts.CosmosExtensions/)
[![Nuget](https://img.shields.io/nuget/vpre/CdIts.CosmosExtensions)](https://www.nuget.org/packages/CdIts.CosmosExtensions/)
[![CI](https://github.com/claasd/CosmosExtensions/actions/workflows/build.yml/badge.svg)](https://github.com/claasd/CosmosExtensions/actions/workflows/build.yml)

CosmosExtensions is a library that provides a set of extension methods for the Azure Cosmos DB SDK LINQ methods, especially for async operations without the need to use .HasMoreResults.

## Examples
```csharp
var item = await container.GetItemLinqQueryable<MyObject>().Where(o => o.Id == "myId").FirstOrDefaultItemAsync();
List<MyObject> allItemsList = await container.GetItemLinqQueryable<MyObject>().ToItemListAsync();
MyObject[] allItemsArray = await container.GetItemLinqQueryable<MyObject>().ToItemArrayAsync();

await foreach(element in container.GetItemLinqQueryable<MyObject>().QueryContainerAsync())
{
    // do something with element
}
```

## Available extension methods

* `QueryContainerAsync()` - Executes a query and returns the results as an IAsyncEnumerable.
* `FirstOrDefaultItemAsync()` - Returns the first item if any item is found, returns `default` only if the iterator reports that there are nor results
* `FirstItemAsync()` - Returns the first item if any item is found throws an exception if there are no results
* `ToItemArrayAsync()` - returns all results as an array
* `ToItemListAsync()` - returns all results as a List

## License
MIT License

