namespace Exemplum.Infrastructure;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence;

public class Seeder
{
    public static async Task SeedDatabase(IServiceProvider appServices, bool deleteDatabase = false)
    {
        using var scope = appServices.CreateScope();

        var serviceProvider = scope.ServiceProvider;
        var logger = serviceProvider.GetRequiredService<ILogger<Seeder>>();

        try
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            if (deleteDatabase)
            {
                logger.LogInformation("Deleting existing database");

                await context.Database.EnsureDeletedAsync();
            }
            
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
            logger.LogError(ex, "An error occurred while migrating or seeding the database");

            throw;
        }
    }    
}