namespace Exemplum.Infrastructure;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence;

public static class Seeder
{
    public static async Task SeedDatabase(IServiceProvider appServices)
    {
        using var scope = appServices.CreateScope();

        var serviceProvider = scope.ServiceProvider;

        try
        {
            
            var logger = serviceProvider.GetRequiredService<Microsoft.Extensions.Logging.ILogger>();
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            
            logger.LogInformation("Ensure database is created");
            
            if (context.Database.IsNpgsql())
            {
                logger.LogInformation("PostgresSql running migrations");
                await context.Database.MigrateAsync();
            }
            else
            {
                logger.LogInformation("SqlLite Database running auto create");
                await context.Database.EnsureCreatedAsync();
            }

            logger.LogInformation("Seeding data");
            
            ApplicationDbContextSeed.SeedDatabase(context);
            
            logger.LogInformation("Data seeded");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while migrating or seeding the database");

            throw;
        }
    }    
}