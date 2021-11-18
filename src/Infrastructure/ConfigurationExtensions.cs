namespace Exemplum.Infrastructure;

using Microsoft.Extensions.Configuration;

public static class ConfigurationExtensions
{
    public static bool UseInMemoryStorage(this IConfiguration config)
    {
        return bool.Parse(config["UseInMemoryStorage"]);
    }
    
    public static bool PublishIntegrationEvents(this IConfiguration config)
    {
        return bool.Parse(config["PublishIntegrationEvents"]);
    }

    public static string GetDefaultConnection(this IConfiguration config)
    {
        return config.GetConnectionString("mssql");
    }
}