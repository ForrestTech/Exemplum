namespace Exemplum.Infrastructure
{
    using Microsoft.Extensions.Configuration;

    public static class ConfigurationExtensions
    {
        public static bool UseInMemoryDatabase(this IConfiguration config)
        {
            return bool.Parse(config["UseInMemoryDatabase"]);
        }
        
        public static string GetDefaultConnection(this IConfiguration config)
        {
            return config.GetConnectionString("Default");
        }
    }
}