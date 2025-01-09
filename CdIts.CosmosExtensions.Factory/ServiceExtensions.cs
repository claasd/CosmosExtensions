using Azure.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CdIts.CosmosExtensions.Factory;

public static class ServiceExtensions
{
    public static IServiceCollection AddCosmosConnector(this IServiceCollection services, string configSection = " cosmos") =>
        services.AddCosmosConnectorInt(null, configSection);

    public static IServiceCollection AddCosmosConnector(this IServiceCollection services, TokenCredential credential,
        string configSection = "cosmos") =>
        services.AddCosmosConnectorInt(credential, configSection);

    private static IServiceCollection AddCosmosConnectorInt(this IServiceCollection services, TokenCredential? credential,
        string configSection = "cosmos")
    {
        services.AddOptions<CosmosConnectorConfig>().Configure((CosmosConnectorConfig options, IConfiguration config) =>
        {
            config.Bind(configSection, options);
            options.Credential = credential;
        });
        services.AddSingleton<CosmosConnector>();
        return services;
    }
}