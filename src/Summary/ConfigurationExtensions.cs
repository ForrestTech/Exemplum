namespace Exemplum.Summary;

using Microsoft.Extensions.Configuration;

public static class ConfigurationExtensions
{
    public static bool SendEmailsToSmtp(this IConfiguration config)
    {
        return bool.Parse(config["SendEmailsToSmtp"]);
    }
}