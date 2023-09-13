namespace Exemplum.Infrastructure;

using Microsoft.Extensions.Configuration;

public static class ConfigurationExtensions
{
    public static bool UseInMemoryStorage(this IConfiguration config)
    {
        return bool.Parse(config["UseInMemoryStorage"] ?? string.Empty);
    }
    
    public static string GetDefaultConnection(this IConfiguration config)
    {
        return config.GetConnectionString("Exemplum") ?? throw new InvalidOperationException();
    }
}