namespace Exemplum.Infrastructure;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence;
using Serilog;

public static class Seeder
{
    public static async Task SeedDatabase(IServiceProvider appServices, bool forceDeleteDatabase = false)
    {
        using var scope = appServices.CreateScope();

        var serviceProvider = scope.ServiceProvider;

        try
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            if (forceDeleteDatabase || context.Database.IsSqlite())
            {
                Log.Information("Deleting existing database");

                await context.Database.EnsureDeletedAsync();
            }
            
            Log.Information("Ensure database is created");
            
            if (context.Database.IsNpgsql())
            {
                Log.Information("PostgresSql running migrations");
                await context.Database.MigrateAsync();
            }
            else
            {
                Log.Information("SqlLite Database running auto create");
                await context.Database.EnsureCreatedAsync();
            }

            Log.Information("Seeding data");
            
            ApplicationDbContextSeed.SeedDatabase(context);
            
            Log.Information("Data seeded");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while migrating or seeding the database");

            throw;
        }
    }    
}