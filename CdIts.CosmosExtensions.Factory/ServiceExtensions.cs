using Azure.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace CdIts.CosmosExtensions.Factory;

public static class ServiceExtensions
{
    public static IServiceCollection AddCosmosConnector(this IServiceCollection services, string configSection = "cosmos") =>
        services.AddCosmosConnectorInt(null, configSection);

    public static IServiceCollection AddCosmosConnector(this IServiceCollection services, JsonSerializerSettings settings, string configSection = "cosmos") =>
        services.AddCosmosConnectorInt(null, configSection, settings);
    
    public static IServiceCollection AddCosmosConnector(this IServiceCollection services, TokenCredential credential,
        string configSection = "cosmos") =>
        services.AddCosmosConnectorInt(credential, configSection);

    
    public static IServiceCollection AddCosmosConnector(this IServiceCollection services, TokenCredential credential,
        JsonSerializerSettings settings, string configSection = "cosmos") =>
        services.AddCosmosConnectorInt(credential, configSection, settings);
    
    private static IServiceCollection AddCosmosConnectorInt(this IServiceCollection services, TokenCredential? credential,
        string configSection = "cosmos", JsonSerializerSettings? settings = null)
    {
        services.AddOptions<CosmosConnectorConfig>().Configure((CosmosConnectorConfig options, IConfiguration config) =>
        {
            config.Bind(configSection, options);
            options.Credential = credential;
            options.Settings = settings;
        });
        services.AddSingleton<CosmosConnector>();
        return services;
    }
}