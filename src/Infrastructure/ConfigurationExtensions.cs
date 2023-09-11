namespace Exemplum.Infrastructure;

using Microsoft.Extensions.Configuration;

public static class ConfigurationExtensions
{
    public static bool UseInMemoryStorage(this IConfiguration config)
    {
        return bool.Parse(config["UseInMemoryStorage"] ?? string.Empty);
    }
    
    public static bool PublishIntegrationEvents(this IConfiguration config)
    {
        return bool.Parse(config["PublishIntegrationEvents"] ?? string.Empty);
    }
    
    public static string GetDefaultConnection(this IConfiguration config)
    {
        return config.GetConnectionString("mssql") ?? throw new InvalidOperationException();
    }
}